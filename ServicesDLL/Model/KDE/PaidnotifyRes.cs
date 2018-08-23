using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.KDE
{
    public class PaidnotifyRes
    {
        public string RetCode { set; get; }
        public string Message { set; get; }
        public string MessageId { set; get; }
        public string Data { set; get; }
        public int Timestamp { set; get; }
        public string OrderNo { set; get; }
    }
}
