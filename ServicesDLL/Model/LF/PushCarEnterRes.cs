using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.LF
{
    public class PushCarEnterRes
    {
        public PushCarEnterHead head { set; get; }
        public PushCarEnterreqData reqData { set; get; }
    }

    public class PushCarEnterHead
    {
        public int cmd { set; get; }
        public int parkinglotId { set; get; }
        public string accessToken { set; get; }
        public string secretKey { set; get; }
        public int ts { set; get; }
    }

    public class PushCarEnterreqData
    {
        public PushCarEnteractInfo actInfo { set; get; }
        public PushCarEnteractData actData { set; get; }
    }

    public class PushCarEnteractInfo
    {
        public string nonce { set; get; }
        public string sign { set; get; }
        public long reqTime { set; get; }
    }

    public class PushCarEnteractData
    {
        public string plateNumber{set;get;}
        public int plateColor{set;get;}
        public long enterTime{set;get;}
        public int vehicleType{set;get;}
        public string enterVedioChannel{set;get;}
        public string enterGateChannel { set; get; }
        public int freeParkingNum{set;get;}
        public string sequenceNumber{set;get;}
        public int parkingIndex{set;get;}
    }
}
