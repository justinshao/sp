using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.PP
{
    public class DetailRes
    {
        public string service { set; get; }
        public string version { set; get; }
        public string charset { set; get; }
        public string result_code { set; get; }
        public string sign { set; get; }
        public string message { set; get; }

        public string plate { set; get; }
        public string card_no { set; get; }
        public string card_id { set; get; }
        public int car_type { set; get; }
        public string car_desc { set; get; }
        public string parking_serial { set; get; }
        public string enter_time { set; get; }
        public string enter_image { set; get; }
        public string enter_gate { set; get; }
        public string enter_security { set; get; }
        public int parking_time { set; get; }
        public string free_time { set; get; }
        public int free_time_value { set; get; }
        public int free_value { set; get; }
        public string leave_time { set; get; }
        public string leave_image { set; get; }
        public string leave_gate { set; get; }
        public string leave_security { set; get; }
        public int total_value { set; get; }
        public Discount discount { set; get; }
        public List<Payments> payments { set; get; }

    }
}
