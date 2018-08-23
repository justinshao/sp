using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.KDE
{
    public class TokenModel
    {
        public string AppId { set; get; }
        public string Nonce { set; get; }
        public string Sign { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
    }
}
