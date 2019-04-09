using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZyGames.Test
{
    public class CaseContext
    {
        public CaseContext()
        {
            SessionId = string.Empty;
        }

        public int UserId { get; set; }

        public string SessionId { get; set; }

        public string PassportId { get; set; }

        public object UserData { get; set; }

        public object Tag { get; set; }
    }
}