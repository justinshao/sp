using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.PP
{
    public class TradeRows
    {
        public string serial { set; get; }
        public short type { set; get; }
        public string trade_time { set; get; }
        public int value { set; get; }
        public string trade_serial { set; get; }
        public int amount { set; get; }
    }
}
