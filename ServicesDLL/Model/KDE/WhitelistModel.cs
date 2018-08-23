using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.KDE
{
    public class WhitelistModel
    {
        public string OperatorId { set; get; }
        public List<WhitelistDataModel> Data { set; get; }
        public string Timestamp { set; get; }
        public string MessageId { set; get; }
    }
}
