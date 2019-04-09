using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.Script;
using ZyGames.Test.Net;
using ZyGames.Test.Head;

namespace ZyGames.Test
{
    public abstract class CaseStep
    {
        public static CaseStep Create(string formatType, string stepName)
        {
            string typeName = string.Format(formatType, stepName);
            string code = string.Format(ConfigUtils.GetSetting("CaseStep.Script.Format", "Case.Step{0}.cs"), stepName);
            var instance = ScriptEngines.Execute(code, typeName);
            //var instance = type.CreateInstance<CaseStep>();
            if (instance == null)
            {
                throw new NullReferenceException(string.Format("Get CaseStep object is null, type:{1}, script code:{0}", code, typeName));
            }
            instance.Action = stepName;
            return instance;
        }

        protected ThreadSession _session;
        protected StepTimer _stepTimer;
        protected IHeadFormater _headFormater;
        private Dictionary<string, string> _params;
        
        protected CaseStep()
        {
            _stepTimer = new StepTimer();
            _headFormater = new SOHeadFormater();
            Action = "";
            _params = new Dictionary<string, string>();
            Runtimes = 1;
        }

        //public string Action { get; private set; }
        public string Action { get; set; }
        
        public StepTimer Timer
        {
            get { return _stepTimer; }
        }

        /// <summary>
        /// Running times of each thread.
        /// </summary>
        public int Runtimes { get; set; }

        /// <summary>
        /// Child run step.
        /// </summary>
        public CaseStep ChildStep { get; private set; }

        protected void SetRequestParam(string key, object value)
        {
            _params[key] = value.ToString();
        }

        protected internal virtual void Init(ThreadSession session)
        {
            _session = session;

            int msgId = _stepTimer.Runtimes + 1;
            SetRequestParam("MsgId", msgId);
            SetRequestParam("Sid", _session.Context.SessionId);
            SetRequestParam("Uid", _session.Context.UserId);
            SetRequestParam("ActionId", Action);
        }


        public virtual byte[] GetRequestData()
        {
            StringBuilder param = new StringBuilder();
            var pairs = _params.ToArray();
            foreach (var pair in pairs)
            {
                if (param.Length > 0)
                {
                    param.Append("&");
                }
                param.AppendFormat("{0}={1}", pair.Key, NetProxy.Encoding(pair.Value));
            }
            return Encoding.UTF8.GetBytes(
                string.Format("?d={0}", NetProxy.GetSign(param.ToString(), _session.Setting.SignKey))
                );
        }

        public virtual void StartRun()
        {
            try
            {
                //todo: connect
                InitConnect();
                SetUrlElement();
                var sendData = GetRequestData();
                for (int i = 0; i < Runtimes; i++)
                {
                    DoRunProcess(sendData);
                }
                DoResult();

            }
            catch (Exception e)
            {
                TraceLog.WriteError("StartRun error:{0}", e);
            }
        }

        private void DoRunProcess(byte[] sendData)
        {
            bool success = false;
            try
            {
                MessageStructure reader = null;
                MessageHead head = null;
                _stepTimer.StartTime();
                _session.Proxy.SendAsync(sendData, data =>
                {
                    try
                    {
                        if (data.Length == 0) return true;
                        reader = new MessageStructure(data);
                        head = _headFormater.GetReadHeader(reader);
                        return head.Action.ToString() == Action;
                    }
                    catch (Exception ex)
                    {
                        TraceLog.WriteError("Step {0} error:{1}", Action, ex);
                        return false;
                    }
                });

                if (CheckCompleted(head) && DecodePacket(reader, head))
                {
                    success = true;
                    _stepTimer.Completed();
                }
            }
            catch (Exception ex)
            {
                _stepTimer.PushError(ex.Message);
            }
            finally
            {
                _stepTimer.StopTime();
            }

            if (success && ChildStep != null)
            {
                ChildStep.StartRun();
            }
        }

        protected virtual bool CheckCompleted(MessageHead head)
        {
            if (head == null || !Equals(head.Action.ToString(), Action))
            {
                _stepTimer.PushError(string.Format("Step {0} pid:{1} request timeout.", Action,
                    _session.Context.PassportId));
                return false;
            }

            if (head.ErrorCode >= 10000)
            {
                _stepTimer.PushError(string.Format("Step {0} pid:{1} request error:{2}-{3}",
                    Action,
                    _session.Context.PassportId,
                    head.ErrorCode,
                    head.ErrorInfo));
                return false;
            }
            return true;
        }

        private void InitConnect()
        {
            _session.InitConnect();
        }

        protected void SetChildStep(string stepName)
        {
            CaseStep caseStep = null;
            if (!string.IsNullOrEmpty(stepName))
            {
                caseStep = Create(_session.Setting.CaseStepTypeFormat, stepName);
                if (caseStep == null) throw new Exception(string.Format(_session.Setting.CaseStepTypeFormat, stepName) + " isn't found.");
                caseStep.Runtimes = 1;
                caseStep.Init(_session);
            }
            ChildStep = caseStep;
        }

        protected abstract void SetUrlElement();

        protected abstract bool DecodePacket(MessageStructure reader, MessageHead head);

        private void DoResult()
        {
            _stepTimer.DoResult();
        }

        protected string EncodePassword(string pwd)
        {
            return new DESAlgorithmNew().EncodePwd(pwd, _session.Setting.EncodePwdKey);
        }
    }
}