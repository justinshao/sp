using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.KDE
{
    public class PhotoModel
    {
        public string AppId { set; get; }
        public string AccessToken { set; get; }
        public string Nonce { set; get; }
        public string Sign { set; get; }
        public string PlateId { set; get; }
        public string[] Image { set; get; }
        public string ImageName { set; get; }
        public string UploadTime { set; get; }
        public string SessionId { set; get; }
        public int InOut { set; get; }
        public int ParkFreeNum { set; get; }

    }
}
