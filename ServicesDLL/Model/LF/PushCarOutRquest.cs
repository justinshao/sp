using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.LF
{
    public class PushCarOutRquest
    {
        public int retCode { set; get; }
        public string retMsg { set; get; }
        public int retCmd { set; get; }
        public PushCarOutRespData respData { set; get; }
    }

    public class PushCarOutRespData
    {

    }
}
