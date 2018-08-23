using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.PP
{
    public class BillingRes
    {
        public string service { set; get; }
        public string version { set; get; }
        public string charset { set; get; }
        public string result_code { set; get; }
        public string message { set; get; }
        public string sign { set; get; }
        public string plate { set; get; }
        public string ticket_formated { set; get; }
        public string parking_serial { set; get; }
        public string parking_order { set; get; }
        public string enter_time { set; get; }
        public int parking_time { set; get; }
        public int total_value { set; get; }
        public int free_time { set; get; }
        public int free_time_value { set; get; }
        public int free_value { set; get; }
        public string free_desc { set; get; }
        public int paid_value { set; get; }
        public int month_card { set; get; }
        public int pay_value { set; get; }
        public int enter_free_time { set; get; }
        public int buffer_time { set; get; }
        public short postpaid { set; get; }
        public int postpaid_credits { set; get; }
        public string parking_number { set; get; }
    }
}
