using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZyGames.Test.Net;
using ZyGames.Framework.RPC.Sockets;

using System.Net;
using System.Net.Sockets;

namespace ZyGames.Test.App
{
    class ExSocketNetProxy : NetProxy
    {
        private Socket _socket;
        private IPEndPoint _address;
        private Func<byte[], bool> _callback;
        private bool _isConnected;

        public string LatestErrorMsg;
        public string LatestReceiveMsg;

        public ExSocketNetProxy(string host, int port)
        {
            var ipAddress = Dns.GetHostAddresses(host).First();
            _address = new IPEndPoint(ipAddress, port);
        }

        public override void CheckConnect()
        {
            if (!_isConnected)
            {
                this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                this._socket.Connect(_address);

                if (this._socket == null || this._socket.Connected == false)
                {
                    StopWait();
                    return;
                }

                _isConnected = true;
            }
        }

        public void s_write(byte[] senddata)
        {
            try
            {
                if (_socket != null && _socket.Connected) //연결상태 유무 확인
                {
                    _socket.Send(senddata, 0, senddata.Length, SocketFlags.None);
                }
                else
                {
                    LatestErrorMsg = "먼저 채팅서버에 접속하세요!";
                    StopWait();
                }
            }
            catch (SocketException se)
            {
                LatestErrorMsg = se.Message.ToString();
                StopWait();
            }
        }

        public Tuple<int, byte[]> s_read()
        {
            try
            {
                byte[] getbyte = new byte[4096];
                var nRecv = _socket.Receive(getbyte, 0, getbyte.Length, SocketFlags.None);
                return new Tuple<int, byte[]>(nRecv, getbyte);
            }
            catch (SocketException se)
            {
                LatestErrorMsg = se.ToString();
            }

            return null;
        }

        async void asyncSendPacket(byte[] data)
        {
            StartWait(RequestTimeout);

            await Task.Run(() => s_write(data));

            try
            {
                byte[] getbyte = new byte[4096];
                var nRecv = _socket.Receive(getbyte, 0, getbyte.Length, SocketFlags.None);


                //return new Tuple<int, byte[]>(nRecv, getbyte);
            }
            catch (SocketException se)
            {
                LatestErrorMsg = se.ToString();
            }
        }

        public override void SendAsync(byte[] data, Func<byte[], bool> callback)
        {
            _callback = callback;

            asyncSendPacket(data);

            //_client.PostSend(data, 0, data.Length);
            StartWait(RequestTimeout);
        }
    }
}
