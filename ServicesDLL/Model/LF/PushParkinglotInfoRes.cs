using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.LF
{
    public class PushParkinglotInfoRes
    {
        public PushParkinglotInfohead head { set; get; }
        public PushParkinglotInforeqData reqData { set; get; }
    }

    public class PushParkinglotInfohead
    {
        public int cmd { set; get; }
        public int parkinglotId { set; get; }
        public string accessToken { set; get; }
        public string secretKey { set; get; }
        public int ts { set; get; }
    }

    public class PushParkinglotInforeqData
    {
        public PushParkinglotInfoactInfo actInfo { set; get; }
        public PushParkinglotInfoactData actData { set; get; }
    }

    public class PushParkinglotInfoactInfo
    {
        public string nonce { set; get; }
        public string sign { set; get; }
        public long reqTime { set; get; }
    }

    public class PushParkinglotInfoactData
    {
        public string parkinglotName{set;get;}
        public string ChargeDesc{set;get;}
        public string ChargeImage{set;get;}
        public int TotalParkingSpace{set;get;}
        public int tempParkingSpace{set;get;}
        public string parkinglotAddr{set;get;}
        public List<ParkinglotChannels> parkinglotChannels{set;get;}
        public string longitude{set;get;}
        public string latitude{set;get;}
        
    }

    public class ParkinglotChannels
    {
        public string id{set;get;}
        public string name{set;get;}

    }

}
