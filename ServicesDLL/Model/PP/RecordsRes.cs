using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.PP
{
    public class RecordsRes
    {
        public string service { set; get; }
        public string version { set; get; }
        public string charset { set; get; }
        public string result_code { set; get; }
        public string sign { set; get; }
        public string message { set; get; }

        public string timestamp { set; get; }
        public List<CarInOutInfo> enters { set; get; }
        public List<CarInOutInfo> leaves { set; get; }
       
    }
}
