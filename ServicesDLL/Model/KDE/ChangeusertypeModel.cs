using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.KDE
{
    public class ChangeusertypeModel
    {
        public string OperatorId { set; get; }
        public List<ChangeusertypeDataModel> Data { set; get; }
        public string Timestamp { set; get; }
        public string MessageId { set; get; }

    }
}
