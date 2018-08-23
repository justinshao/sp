using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.PP
{
    public class Discount
    {
        public int type { set; get; }
        public int value { set; get; }
        public string reason { set; get; }
        public string identity { set; get; }
        public string voucher { set; get; }

    }
}
