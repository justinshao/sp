using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.DHWY
{
    public class OutCXJFModel
    {
        public string orderid { set; get; }
        public string local_orderid { set; get; }
        public float total_price { set; get; }
        public float paid_price { set; get; }
        public float discount_amount { set; get; }
        public float discount_price { set; get; }
        public float unpay_price { set; get; }
        public int total_count { set; get; }
        public int pay_time { set; get; }
        public int query_time { set; get; }
    }
}
