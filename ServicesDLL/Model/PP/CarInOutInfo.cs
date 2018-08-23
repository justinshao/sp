using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.PP
{
    public class CarInOutInfo
    {
        public string plate { set; get; }
        public int car_type { set; get; }
        public string car_desc { set; get; }
        public int vip { set; get; }
        public string card_no { set; get; }
        public string card_id { set; get; }
        public string parking_serial { set; get; }
        public string enter_time { set; get; }
        public string enter_image { set; get; }
        public string enter_gate { set; get; }
        public string enter_security { set; get; }
        public string leave_time { set; get; }
        public string leave_image { set; get; }
        public string leave_gate { set; get; }
        public string leave_security { set; get; }
        public int parking_time { set; get; }
        public int total_value { set; get; }
        public int free_value { set; get; }
        public int online_value { set; get; }
        public int cash_value { set; get; }
        public int postpaid_value { set; get; }
    }
}
