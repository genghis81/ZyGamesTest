using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using ZyGames.Test.Net;

namespace ZyGames.Test
{
    public class ThreadSession
    {
        private NetProxy _netProxy;

        public ThreadSession(int id = 0)
        {
            Id = id;
            Context = new CaseContext();
        }

        public TaskSetting Setting { get; set; }

        /// <summary>
        /// connect not timer.
        /// </summary>
        public void InitConnect()
        {
            if (_netProxy == null)
            {
                _netProxy = NetProxy.Create(Setting.Url);
            }
            _netProxy.CheckConnect();
        }

        public NetProxy Proxy { get { return _netProxy; } }

        public int Id { get; set; }

        public CaseContext Context { get; set; }

    }
}