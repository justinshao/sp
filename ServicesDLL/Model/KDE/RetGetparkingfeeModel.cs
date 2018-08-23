using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.KDE
{
    public class RetGetparkingfeeModel
    {
        public string SessionId { set; get; }
        public string PlateId { set; get; }
        public string InTime { set; get; }
        public string CutOffTime { set; get; }
        public int TotalCharge { set; get; }
        public int PayCharge { set; get; }
        public int PaidCharge { set; get; }
        public int DiscountCharge { set; get; }
        public int DiscountTime { set; get; }
        public int ParkingTime { set; get; }
        public int PaidFreeTime { set; get; }
        public string Memo { set; get; }
    }
}
