using System;
using System.IO;
using System.Net;

namespace ZyGames.Test.Net
{
    internal class HttpNetProxy : NetProxy
    {
        private readonly string _url;

        public HttpNetProxy(string url)
        {
            _url = url;
        }

        public override void SendAsync(byte[] data, Func<byte[], bool> callback)
        {
            var request = (HttpWebRequest)WebRequest.Create(_url);
            request.Method = "POST";
            request.ContentType = "application/octetstream"; 
            request.ContentLength = data.Length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
                stream.Close();
            }
            byte[] result = new byte[0];
            WebResponse resp = request.GetResponse();
            using (Stream stream = resp.GetResponseStream())
            {
                if (stream != null)
                {
                    var reader = new BinaryReader(stream);
                    result = ReadStream(reader);
                }
            }
            callback(result);
        }

    }
}