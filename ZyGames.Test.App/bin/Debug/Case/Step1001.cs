using System;
using System.Text;
using System.Collections.Generic;

using ZyGames.Framework.Common;
using ZyGames.Framework.RPC.IO;

namespace ZyGames.Test.App.Case
{
    /// <summary>
    /// 登录
    /// </summary>
    public class Step1001 : CaseStep
    {
        public override byte[] GetRequestData()
        {
            byte[] Body = Encoding.Unicode.GetBytes("1");

            List<byte> dataSource = new List<byte>();
            dataSource.AddRange(BitConverter.GetBytes((Int32)PACKETID.REQ_ECHO));
            dataSource.AddRange(BitConverter.GetBytes((Int16)0));
            dataSource.AddRange(BitConverter.GetBytes((Int16)0));
            dataSource.AddRange(BitConverter.GetBytes(Body.Length));
            dataSource.AddRange(Body);

            return dataSource.ToArray();
        }

        protected override void SetUrlElement()
        {
        }

        protected override bool DecodePacket(MessageStructure reader, MessageHead head)
        {
            int msgLenth = reader.ReadInt();
            string msgBody = reader.ReadString();

            _session.Context.SessionId = msgBody;

            return true;
        }

    }
}