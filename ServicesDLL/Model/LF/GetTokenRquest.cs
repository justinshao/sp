using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.LF
{
    public class GetTokenRquest
    {
        public int retCode{set;get;}
        public string retMsg{set;get;}
        public int retCmd{set;get;}
        public GetTokenRquestrespData respData { set; get; }
    }

    public class GetTokenRquestrespData
    {
        public int ts { set; get; }
        public string accessToken { set; get; }
        public string refreshToken { set; get; }
    }
}
