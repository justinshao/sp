using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.PP
{
    public class TradeListRes
    {
        public string service { set; get; }
        public string version { set; get; }
        public string charset { set; get; }
        public string result_code { set; get; }
        public string message { set; get; }
        public string sign { set; get; }
        public int total_count { set; get; }
        public List<TradeRows> rows { set; get; }
    }
}
