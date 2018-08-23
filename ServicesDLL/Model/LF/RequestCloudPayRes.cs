using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.LF
{
    public class RequestCloudPayRes
    {
        public RequestCloudPayHead head { set; get; }
        public RequestCloudPayReqData reqData { set; get; }
    }

    public class RequestCloudPayHead
    {
        public int cmd { set; get; }
        public int parkinglotId { set; get; }
        public string accessToken { set; get; }
        public string secretKey { set; get; }
        public int ts { set; get; }
    }

    public class RequestCloudPayReqData
    {
        public RequestCloudPayActInfo actInfo { set; get; }
        public RequestCloudPayActData actData { set; get; }
    }

    public class RequestCloudPayActInfo
    {
        public string nonce { set; get; }
        public string sign { set; get; }
        public long reqTime { set; get; }
    }

    public class RequestCloudPayActData
    {
        public string plateNumber{set;get;}
        public string sequenceNumber{set;get;}
        public int needParkingFee{set;get;}
        public int onLineRealPaid { set; get; }
        public int offLinePaid { set; get; }
        public int couponFee { set; get; }
        public int couponTime { set; get; }
        public string couponCode{set;get;}
        public int curNeedParkingFee { set; get; }
        public int parkingTime { set; get; }
        public string rechargeIndex{set;get;}
      

    }
}
