using ZyGames.Framework.Common;
using ZyGames.Framework.RPC.IO;

namespace ZyGames.Test.App.Case
{
    /// <summary>
    /// 登录
    /// </summary>
    public class Step1004 : CaseStep
    {
        protected override void SetUrlElement()
        {
            _session.Context.PassportId = "Z" + (_session.Setting.PassprotId + _session.Id);
            string pwd = EncodePassword(_session.Setting.UserPwd);
            SetRequestParam("MobileType", 1);
            SetRequestParam("Pid", _session.Context.PassportId);
            SetRequestParam("Pwd", pwd);
            SetRequestParam("DeviceID", "0a-0s-0f-04");
            SetRequestParam("GameType", _session.Setting.GameId);
            SetRequestParam("ServerID", _session.Setting.ServerId);
            SetRequestParam("ScreenX", "500");
            SetRequestParam("ScreenY", "400");
            SetRequestParam("RetailID", "0000");
            SetRequestParam("RetailUser", "");
            SetRequestParam("ClientAppVersion", "1");

        }

        protected override bool DecodePacket(MessageStructure reader, MessageHead head)
        {
            _session.Context.SessionId = reader.ReadString();
            _session.Context.UserId = reader.ReadString().ToInt();
            int UserType = reader.ReadInt();
            string LoginTime = reader.ReadString();
            int GuideID = reader.ReadInt();
            if (GuideID == 1005)
            {
                SetChildStep("1005");
            }
            return true;
        }

    }
}