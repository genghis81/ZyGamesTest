using System;
using ZyGames.Framework.RPC.IO;


namespace ZyGames.Test.Head
{
    public interface IHeadFormater
    {
        MessageHead GetReadHeader(MessageStructure reader);
    }
}
