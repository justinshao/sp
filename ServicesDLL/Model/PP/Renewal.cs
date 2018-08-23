using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.PP
{
    public class Renewal
    {
        public string service { set; get; }
        public string version { set; get; }
        public string charset { set; get; }
        public string sign { set; get; }
        public string merchant { set; get; }
        public string park_uuid { set; get; }
        public string plate { set; get; }
        public string card_no { set; get; }
        public string card_id { set; get; }
        public string renewal_time { set; get; }
        public string pay_serial { set; get; }
        public int pay_value { set; get; }
        public int type { set; get; }
        public int value { set; get; }
    }
}
