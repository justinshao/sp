using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.KDE
{
    public class LockcarModel
    {
        public string UserId { set; get; }
        public string PlateId { set; get; }
        public int ActionFlag { set; get; }
        public string Timestamp { set; get; }
        public string MessageId { set; get; }
    }
}
