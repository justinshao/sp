using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.KDE
{
    public class ParkInfoModel
    {
        public string AppId { set; get; }
        public string AccessToken { set; get; }
        public string Nonce { set; get; }
        public string Sign { set; get; }
        public string ParkId { set; get; }
        public string ParkName { set; get; }
        public string Longitude { set; get; }
        public string Latitude { set; get; }
        public string ChargeScheme { set; get; }
        public string ChargeSchemeImg { set; get; }
        public string TotalBerth { set; get; }
        public string TempBerth { set; get; }
        public string Address { set; get; }
        public List<ChannelModel> Channel { set; get; }
    }
}
