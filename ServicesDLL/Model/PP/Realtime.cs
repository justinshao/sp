using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.PP
{
    public class Realtime
    {
        public string service { set; get; }
        public string version { set; get; }
        public string charset { set; get; }
        public string sign { set; get; }
        public string merchant { set; get; }
        public string park_uuid { set; get; }

    }
}
