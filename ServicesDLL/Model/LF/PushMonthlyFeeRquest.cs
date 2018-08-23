using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.LF
{
    public class PushMonthlyFeeRquest
    {
        public int retCode { set; get; }
        public string retMsg { set; get; }
        public int retCmd { set; get; }
        public PushMonthlyFeeRespData respData { set; get; }
    }

    public class PushMonthlyFeeRespData
    {
    }
}
