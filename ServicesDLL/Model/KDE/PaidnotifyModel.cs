using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.KDE
{
    public class PaidnotifyModel
    {
        public string SessionId { set; get; }
        public string PlateId { set; get; }
        public int PayCharge { set; get; }
        public int RealCharge { set; get; }
        public string PaidTime { set; get; }
        public string CutOffTime { set; get; }
        public string DiscountTime { set; get; }
        public int DiscountCharge { set; get; }
        public string DiscountCode { set; get; }
        public string OrderNo { set; get; }
        public string Timestamp{set;get;}
        public string MessageId { set; get; }
    }
}
