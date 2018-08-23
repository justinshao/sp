using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.PP
{
    public class DiscountDestroy
    {
        public string service { set; get; }
        public string version { set; get; }
        public string charset { set; get; }
        public string sign { set; get; }
        public string merchant { set; get; }
        public string park_uuid { set; get; }
        public string card_no { set; get; }
        public string card_id { set; get; }
        public string car_plate { set; get; }
        public string ticket { set; get; }
        public string grant_serial { set; get; }
        public string parking_serial { set; get; }
    }
}
