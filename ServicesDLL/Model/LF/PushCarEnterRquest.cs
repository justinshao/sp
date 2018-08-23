using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.LF
{
    public  class PushCarEnterRquest
    {
        public int retCode { set; get; }
        public string retMsg { set; get; }
        public int retCmd { set; get; }
        public PushCarEnterrespData respData { set; get; }
    }

    public class PushCarEnterrespData
    {

    }
}
