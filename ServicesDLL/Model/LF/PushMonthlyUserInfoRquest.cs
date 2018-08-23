using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.LF
{
    public class PushMonthlyUserInfoRquest
    {
        public int retCode { set; get; }
        public string retMsg { set; get; }
        public int retCmd { set; get; }
        public PushMonthlyUserInfoRespData respData { set; get; }
    }

    public class PushMonthlyUserInfoRespData
    {
    }
}
