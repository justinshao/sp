using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.PP
{
    public class DiscountQueryRes
    {
        public string service { set; get; }
        public string version { set; get; }
        public string charset { set; get; }
        public string result_code { set; get; }
        public string sign { set; get; }
        public string message { set; get; }
        public string card_no { set; get; }
        public string card_id { set; get; }
        public string plate { set; get; }
        public string parking_serial { set; get; }
        public int type { set; get; }
        public int value { set; get; }
        public string reason { set; get; }
        public string identity { set; get; }
        public int enforce { set; get; }
        public string voucher { set; get; }
    }
}
