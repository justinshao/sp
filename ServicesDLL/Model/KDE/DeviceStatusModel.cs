using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.KDE
{
    public class DeviceStatusModel
    {
        public string AppId { set; get; }
        public string AccessToken { set; get; }
        public string Nonce { set; get; }
        public string Sign { set; get; }
        public string Passageway { set; get; }
        public int DeviceType { set; get; }
        public int OnlineStatus { set; get; }
        public int DeviceStatus { set; get; }
        public string Description { set; get; }
    }
}
