using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.PP
{
    public class Payments
    {
        public string parking_serial { set; get; }
        public string parking_order { set; get; }
        public string pay_serial { set; get; }
        public int pay_type { set; get; }
        public string pay_time { set; get; }
        public int value { set; get; }
    }
}
