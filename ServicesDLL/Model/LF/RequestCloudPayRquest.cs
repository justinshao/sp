using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.LF
{
    public  class RequestCloudPayRquest
    {
        public int retCode { set; get; }
        public string retMsg { set; get; }
        public int retCmd { set; get; }
        public RequestCloudPayRespData respData { set; get; }
    }

    public class RequestCloudPayRespData
    {
        public int leftNeedFee {set;get;}
        public int cloudPaid{set;get;}
        public int vipBalance{set;get;}
    }
}
