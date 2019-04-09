using ZyGames.Framework.RPC.IO;

namespace ZyGames.Test.App.Case
{
    /// <summary>
    /// 创角
    /// </summary>
    public class Step1005 : CaseStep
    {
        protected override void SetUrlElement()
        {
            int number = _session.Setting.PassprotId + _session.Id;
            var pid = "Z" + number;
            SetRequestParam("UserName", "NName" + number);
            SetRequestParam("Sex", 1);
            SetRequestParam("HeadID", 0);
            SetRequestParam("RetailID", "0000");
            SetRequestParam("Pid", pid);
            SetRequestParam("MobileType", 1);
            SetRequestParam("DeviceID", "");
            SetRequestParam("ScreenX", 100);
            SetRequestParam("ScreenY", 100);
            SetRequestParam("ClientAppVersion", 1);
            SetRequestParam("ProfessionType", 1);
            SetRequestParam("PointX", 100);
            SetRequestParam("PointY", 100);

        }

        protected override bool DecodePacket(MessageStructure reader, MessageHead head)
        {
            _session.Context.UserId = reader.ReadInt();
            return true;
        }

    }
}