using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using ZyGames.Framework.Common.Configuration;

namespace ZyGames.Test.Net
{
    public abstract class NetProxy
    {
        /// <summary>
        /// Config request timeout(3s).
        /// </summary>
        public static int RequestTimeout = ConfigUtils.GetSetting("Request.Timeout", 3000);

        //public static string Encoding(string str)
        //{
        //    return HttpUtility.UrlEncode(str);
        //}

        public static string GetSign(string param, string signKey)
        {
            string sign = "";
            if (!string.IsNullOrEmpty(signKey))
            {
                sign = FormsAuthentication.HashPasswordForStoringInConfigFile(param + signKey, "MD5").ToLower();
            }
            return HttpUtility.UrlEncode(string.Format("{0}&sign={1}", param, sign));
        }

        public static NetProxy Create(string remoteAddress)
        {
            NetProxy session = null;
            var address = (remoteAddress ?? "").Split(':');
            if (address.Length > 1 && !address[0].StartsWith("http", StringComparison.CurrentCultureIgnoreCase))
            {
                session = new SocketNetProxy(address[0], Convert.ToInt32(address[1]));
            }
            else
            {
                session = new HttpNetProxy(remoteAddress);
            }
            return session;
        }

        private ManualResetEvent singal;

        protected NetProxy()
        {
            singal = new ManualResetEvent(false);
        }

        protected const int BufferSize = 512;

        public virtual void CheckConnect()
        {
        }

        public void StartWait(int timeout)
        {
            singal.Reset();
            if (timeout <= 0)
            {
                singal.WaitOne();
            }
            else
            {
                singal.WaitOne(timeout);
            }
        }

        public void StopWait()
        {
            singal.Set();
        }

        public abstract void SendAsync(byte[] data, Func<byte[], bool> callback);

        public static byte[] ReadStream(BinaryReader reader)
        {
            List<byte[]> buffers = new List<byte[]>();
            int size = 0;
            while (true)
            {
                var buffer = new byte[BufferSize];
                size = reader.Read(buffer, 0, buffer.Length);
                if (size == 0)
                {
                    break;
                }
                byte[] data = new byte[size];
                Buffer.BlockCopy(buffer, 0, data, 0, size);
                buffers.Add(data);
            }
            return CombineByte(buffers);
        }

        protected static byte[] CombineByte(ICollection<byte[]> buffers)
        {
            Int32 length = buffers.Sum(tempbyte => tempbyte.Length);
            Byte[] result = new Byte[length];
            Int32 offset = 0;
            foreach (byte[] buffer in buffers)
            {
                Buffer.BlockCopy(buffer, 0, result, offset, buffer.Length);
                offset += buffer.Length;
            }
            return result;
        }
    }
}