using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.KDE
{
    public class LockcarRes
    {
        public string RetCode { set; get; }
        public string Message { set; get; }
        public string MessageId { set; get; }
        public string Data { set; get; }
        public int LockStatus { set; get; }
    }
}
