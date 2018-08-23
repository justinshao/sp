using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.PP
{
    public class TradeList
    {
        public string service { set; get; }
        public string version { set; get; }
        public string charset { set; get; }
        public string sign { set; get; }
        public string merchant { set; get; }
        public string park_uuid { set; get; }
        public string plate { set; get; }
        public string card_no { set; get; }
        public int page_index { set; get; }
        public int page_size { set; get; }
        public short type { set; get; }
    }
}
