using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.KDE
{
    public class RefreshModel
    {
        public string AppId { set; get; }
        public string Nonce { set; get; }
        public string Sign { set; get; }
        public string RefreshToken { set; get; }
    }
}
