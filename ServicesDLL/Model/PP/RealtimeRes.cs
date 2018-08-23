using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.PP
{
    public class RealtimeRes
    {
        public string service { set; get; }
        public string version { set; get; }
        public string charset { set; get; }
        public string result_code { set; get; }
        public string sign { set; get; }
        public string message { set; get; }
        public int total { set; get; }
        public int parking { set; get; }
        public int available { set; get; }
        public int enter_times { set; get; }
        public int leave_times { set; get; }
        public int cash_fee { set; get; }
        public int charge_fee { set; get; }


    }
}
