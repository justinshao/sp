using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.KDE
{
    public class MonthlyUserRechargeModel
    {
        public string AppId { set; get; }
        public string AccessToken { set; get; }
        public string Nonce { set; get; }
        public string Sign { set; get; }
        public string Telephone { set; get; }
        public string UserName { set; get; }
        public string Address { set; get; }
        public string IdCard { set; get; }
        public string PlateId { set; get; }
        public string BeginTime { set; get; }
        public string EndTime { set; get; }
        public string RechargeId { set; get; }
        public int PayCharge { set; get; }
        public int PayType { set; get; }
        public string OperatorId { set; get; }
    }
}
