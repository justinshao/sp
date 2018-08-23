using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.LF
{
    public class PushCarOutRes
    {
        public PushCarOuthead head { set; get; }
        public PushCarOutreqData reqData { set; get; }
    }

    public class PushCarOuthead
    {
        public int cmd { set; get; }
        public int parkinglotId { set; get; }
        public string accessToken { set; get; }
        public string secretKey { set; get; }
        public int ts { set; get; }
    }

    public class PushCarOutreqData
    {
        public PushCarOutactInfo actInfo { set; get; }
        public PushCarOutactData actData { set; get; }
    }

    public class PushCarOutactInfo
    {
        public string nonce { set; get; }
        public string sign { set; get; }
        public long reqTime { set; get; }
    }

    public class PushCarOutactData
    {
        public string plateNumber{set;get;}
        public int plateColor{set;get;}
        public long enterTime{set;get;}
        public string outTime{set;get;}
        public int vehicleType { set; get; }
        public string enterVedioChannel{set;get;}
        public string enterGateChannel{set;get;}
        public string outVedioChannel{set;get;}
        public string outGateChannel{set;get;}
        public int freeParkingNum{set;get;}
        public int needParkingFee{set;get;}
        public int realParkingFee { set; get; }
        public string couponCode{set;get;}
        public int couponParkingFee{set;get;}
        public int couponParkingTime{set;get;}
        public int parkingTime{set;get;}
        public string sequenceNumber{set;get;}
        public int parkingIndex{set;get;}

    }
}
