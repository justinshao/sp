using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.LF
{
    public class RefreshTokenRes
    {
        public RefreshTokenHead head { set; get; }
        public RefreshTokenReqData reqData { set; get; }
    }

    public class RefreshTokenHead
    {
        public int cmd { set; get; }
        public int parkinglotId { set; get; }
        public string accessToken { set; get; }
        public string secretKey { set; get; }
        public int ts { set; get; }
    }

    public class RefreshTokenReqData
    {
        public RefreshTokenActInfo actInfo { set; get; }
        public RefreshTokenActData actData { set; get; }
    }

    public class RefreshTokenActInfo
    {
        public string nonce { set; get; }
        public string sign { set; get; }
        public long reqTime { set; get; }
    }

    public class RefreshTokenActData
    {
        public string refreshToken { set; get; }
    }
}
