using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.LF
{
    public class RefreshTokenRquest
    {
        public int retCode { set; get; }
        public string retMsg { set; get; }
        public int retCmd { set; get; }
        public RefreshTokenRespData respData { set; get; }
    }

    public class RefreshTokenRespData
    {
        public string accessToken { set; get; }
    }
}
