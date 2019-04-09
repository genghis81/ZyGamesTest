using ZyGames.Framework.RPC.IO;

namespace ZyGames.Test.Head
{
    public class DefaultHeadFormater : IHeadFormater
    {
        public MessageHead GetReadHeader(MessageStructure reader)
        {
            return reader.ReadHeadGzip();
        }
    }
}
