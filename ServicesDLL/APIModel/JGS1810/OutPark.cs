using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.APIModel.JGS1810
{
    public  class OutPark
    {
        public string id { set; get; }
        public string plate_num { set; get; }
        public string plate_type { set; get; }
        public string out_time { set; get; }
        public string in_time { set; get; }
        public float sum_time { set; get; }
        public float sum_money { set; get; }
        public string out_gate { set; get; }
        public string out_img { set; get; }
    }
}
