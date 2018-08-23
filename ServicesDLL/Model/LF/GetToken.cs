using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.LF
{
    public class GetToken
    {
        public GetTokenHeadData head { set; get; }
        public GetTokenReqData reqData { set; get; }
    }

    public class GetTokenHeadData
    {
        public int cmd { set; get; }
        public int parkinglotId { set; get; }
        public string accessToken { set; get; }
        public string secretKey { set; get; }
        public int ts { set; get; }

    }

    public class GetTokenReqData
    {
        public GetTokenActInfo actInfo { set; get; }
        public GetTokenActData actData { set; get; }
    }

    public class GetTokenActInfo
    {
        public string nonce { set; get; }
        public string sign { set; get; }
        public long reqTime { set; get; }

    }

    public class GetTokenActData
    {

    }
}
