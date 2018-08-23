using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.LF
{
    public class PushOffLinePayRes
    {
        public PushOffLinePayhead head { set; get; }
        public PushOffLinePayreqData reqData { set; get; }
    }

    public class PushOffLinePayhead
    {
        public int cmd { set; get; }
        public int parkinglotId { set; get; }
        public string accessToken { set; get; }
        public string secretKey { set; get; }
        public int ts { set; get; }
    }

    public class PushOffLinePayreqData
    {
        public PushOffLinePayactInfo actInfo { set; get; }
        public PushOffLinePayactData actData { set; get; }
    }

    public class PushOffLinePayactInfo
    {
        public string nonce { set; get; }
        public string sign { set; get; }
        public long reqTime { set; get; }
    }

    public class PushOffLinePayactData
    {
        public string plateNumber{set;get;}
        public string sequenceNumber{set;get;}
        public int needParkingFee{set;get;}
        public int onLinePaid{set;get;}
        public int offLinePaid{set;get;}
        public int couponFee{set;get;}
        public int couponTime{set;get;}
        public string couponCode{set;get;}
        public int curNeedParkingFee{set;get;}
        public long enterTime{set;get;}
        public long outTime{set;get;}
        public string offLinePaidNo{set;get;}
        public string invoiceCode{set;get;}
        public long parkingTime{set;get;}
        public int payType{set;get;}
        public string freeDesc{set;get;}
        public int freeFee{set;get;}
        public int rechargeIndex{set;get;}
    }
}
