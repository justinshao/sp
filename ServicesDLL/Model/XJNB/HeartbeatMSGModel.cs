using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.XJNB
{
    public class HeartbeatMSGModel
    {
        public string devId { set; get; }
        public string devName { set; get; }
        public string status { set; get; }
        public string errorInfo { set; get; }
        public string errorTime { set; get; }
    }
}
