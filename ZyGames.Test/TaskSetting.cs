using System;
using System.Collections.Generic;
using ZyGames.Framework.Common.Configuration;

namespace ZyGames.Test
{
    public class TaskSetting
    {
        private static string _caseStepTypeFormat;
        private static string _testUrl;
        private static int _testGameId;
        private static int _testServerId;
        private static string _testSignKey;
        private static int _testThreadNum;
        private static int _testRuntimes;
        private static string[] _testSteps;
        private static string _testEncodePwdKey;
        private static int _testPassprotId;
        private static int _testUserId;
        private static string _testUserPwd;

        static TaskSetting()
        {
            _caseStepTypeFormat = ConfigUtils.GetSetting("CaseStep.Type.Format", "");
            _testUrl = ConfigUtils.GetSetting("Test.Url", "");
            _testEncodePwdKey = ConfigUtils.GetSetting("Test.EncodePwdKey", "j6=9=1ac");
            _testGameId = ConfigUtils.GetSetting("Test.GameId", 1);
            _testServerId = ConfigUtils.GetSetting("Test.ServerId", 1);
            _testSignKey = ConfigUtils.GetSetting("Test.SignKey", "");

            _testThreadNum = ConfigUtils.GetSetting("Test.ThreadNum", 100);
            _testRuntimes = ConfigUtils.GetSetting("Test.Runtimes", 1);
            string steps = ConfigUtils.GetSetting("Test.Steps", "");
            _testSteps = string.IsNullOrEmpty(steps) ? new string[0] : steps.Split(',');
            _testPassprotId = ConfigUtils.GetSetting("Test.Pid", 10000);
            _testUserId = ConfigUtils.GetSetting("Test.Uid", 1380000);
            _testUserPwd = ConfigUtils.GetSetting("Test.UserPwd", "");
        }

        public TaskSetting()
        {
            WaitTimeout = TimeSpan.Zero;
            CaseStepTypeFormat = _caseStepTypeFormat;
            EncodePwdKey = _testEncodePwdKey;
            Url = _testUrl;
            GameId = _testGameId;
            ServerId = _testServerId;
            SignKey = _testSignKey;
            ThreadNum = _testThreadNum;
            Runtimes = _testRuntimes;
            PassprotId = _testPassprotId;
            UserId = _testUserId;
            UserPwd = _testUserPwd;
            CaseStepList = new List<string>(_testSteps);
        }

        public string CaseStepTypeFormat { get; set; }

        public string EncodePwdKey { get; set; }

        public string Url { get; set; }

        public int GameId { get; set; }

        public int ServerId { get; set; }

        public string SignKey { get; set; }

        /// <summary>
        /// Run thread number.
        /// </summary>
        public int ThreadNum { get; set; }

        /// <summary>
        /// Running times of each thread.
        /// </summary>
        public int Runtimes { get; set; }

        public int PassprotId { get; set; }

        public int UserId { get; set; }

        public string UserPwd { get; set; }
        /// <summary>
        /// Wait task timeout.
        /// </summary>
        public TimeSpan WaitTimeout { get; set; }

        public List<string> CaseStepList { get; private set; }

    }
}