using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.LF
{
    public class PushParkinglotInfoRquest
    {
        public int retCode { set; get; }
        public string retMsg { set; get; }
        public int retCmd { set; get; }
        public PushParkinglotInfoRquestRespData respData { set; get; }
    }

    public class PushParkinglotInfoRquestRespData
    {

    }
}
