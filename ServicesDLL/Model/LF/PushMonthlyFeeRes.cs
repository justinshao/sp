using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.LF
{
    public class PushMonthlyFeeRes
    {
        public PushMonthlyFeeHead head { set; get; }
        public PushMonthlyFeeReqData reqData { set; get; }
    }

    public class PushMonthlyFeeHead
    {
        public int cmd { set; get; }
        public int parkinglotId { set; get; }
        public string accessToken { set; get; }
        public string secretKey { set; get; }
        public int ts { set; get; }
    }

    public class PushMonthlyFeeReqData
    {
        public PushMonthlyFeeActInfo actInfo { set; get; }
        public PushMonthlyFeeActData actData { set; get; }
    }

    public class PushMonthlyFeeActInfo
    {
        public string nonce { set; get; }
        public string sign { set; get; }
        public long reqTime { set; get; }
    }

    public class PushMonthlyFeeActData
    {
        public string userCellNumber{set;get;}
        public string userName{set;get;}
        public string userAddr{set;get;}
        public string userIDCardNo{set;get;}
        public string plateNumber{set;get;}
        public string monthlyCardID { set; get; }
        public long beginTime { set; get; }
        public long endTime { set; get; }
        public string rechargeID { set; get; }
        public int chargeFee { set; get; }
        public int chargeType { set; get; }
        public long chargeTime { set; get; }
        public string operatorId{set;get;}
    }
}
