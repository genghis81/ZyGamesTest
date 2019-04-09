using ZyGames.Framework.RPC.IO;


namespace ZyGames.Test.Head
{
    public class SOHeadFormater : IHeadFormater
    {
        public MessageHead GetReadHeader(MessageStructure reader)
        {
            MessageHead rtnHeader = new MessageHead();

            int msgType = reader.ReadInt();
            if (msgType == 0)
            {
                return rtnHeader;
            }

            rtnHeader = new MessageHead(msgType, 0, "");
            
            return rtnHeader;
        }
    }
}
