using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ZyGames.Framework.RPC.Sockets;

namespace ZyGames.Test.Net
{
    internal class SocketNetProxy : NetProxy
    {
        private ClientSocket _client;
        private IPEndPoint _address;
        private Func<byte[], bool> _callback;
        private bool _isConnected;
        private ClientSocketSettings setting;

        public SocketNetProxy(string host, int port)
        {
            var ipAddress = Dns.GetHostAddresses(host).First();
            _address = new IPEndPoint(ipAddress, port);
            setting = new ClientSocketSettings(BufferSize, _address);

        }

        private void DoClosed(object sender, SocketEventArgs e)
        {
            _isConnected = false;
            StopWait();
        }

        private void DoReceived(object sender, SocketEventArgs e)
        {
            if (_callback != null)
            {
                if (_callback(e.Data))
                {
                    StopWait();
                }
            }
        }

        public override void CheckConnect()
        {
            if (!_isConnected)
            {
                _client = new ClientSocket(setting);
                _client.DataReceived += DoReceived;
                _client.Disconnected += DoClosed;
                _client.Connect();
                _isConnected = true;
            }
        }

        public override void SendAsync(byte[] data, Func<byte[], bool> callback)
        {
            _callback = callback;
            _client.PostSend(data, 0, data.Length);
            //StartWait(RequestTimeout);
        }

    }
}