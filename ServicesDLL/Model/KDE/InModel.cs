using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.KDE
{
    public class InModel
    {
        public string AppId { set; get; }
        public string AccessToken { set; get; }
        public string Nonce { set; get; }
        public string Sign { set; get; }
        public string PlateId { set; get; }
        public int PlateColor { set; get; }
        public string InTime { set; get; }
        public int CarType { set; get; }
        public string InVideoId { set; get; }
        public string InGateId { set; get; }
        public string SessionId { set; get; }
        public int ParkFreeNum { set; get; }
        public int Confidence { set; get; }
    }
}
