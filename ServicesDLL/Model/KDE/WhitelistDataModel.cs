using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.KDE
{
    public class WhitelistDataModel
    {
        public string PlateId { set; get; }
        public string Type { set; get; }
        public string CarOwner { set; get; }
        public string IdCard { set; get; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public string PlateColor { set; get; }
        public string EffectiveTime { set; get; }
        public string ExpiredTime { set; get; }
        public string Memo { set; get; }
    }
}
