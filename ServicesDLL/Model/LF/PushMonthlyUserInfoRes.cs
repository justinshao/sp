using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.LF
{
    public class PushMonthlyUserInfoRes
    {
        public PushMonthlyUserInfoHead head { set; get; }
        public PushMonthlyUserInfoReqData reqData { set; get; }
    }
    public class PushMonthlyUserInfoHead
    {
        public int cmd { set; get; }
        public int parkinglotId { set; get; }
        public string accessToken { set; get; }
        public string secretKey { set; get; }
        public int ts { set; get; }
    }

    public class PushMonthlyUserInfoReqData
    {
        public PushMonthlyUserInfoActInfo actInfo { set; get; }
        public PushMonthlyUserInfoActData actData { set; get; }
    }

    public class PushMonthlyUserInfoActInfo
    {
        public string nonce { set; get; }
        public string sign { set; get; }
        public long reqTime { set; get; }
    }

    public class PushMonthlyUserInfoActData
    {
        public string userCellNumber{set;get;}
        public string userName{set;get;}
        public string userAddr{set;get;}
        public string userIDCardNo{set;get;}
        public string plateNumber{set;get;}
        public string monthlyCardID{set;get;}
        public long beginTime { set; get; }
        public long endTime { set; get; }
        public string platePlaceNo{set;get;}
        public long getMonthlyCardTime{set;get;}


    }

}
