using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.PP
{
    public class Parks
    {
        public string uuid { set; get; }
        public string name { set; get; }
        public int type { set; get; }
        public string address { set; get; }
        public double latitude { set; get; }
        public double longitude { set; get; }
        public int charge { set; get; }
        public string charge_desc { set; get; }
        public string charge_simple_desc { set; get; }
        public int month_card { set; get; }
        public string street_view { set; get; }
    }
}
