using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.XJNB
{
    public class HeartbeatModel
    {
        public string refId { set; get; }
        public string time { set; get; }
        public string nextTime { set; get; }
        public List<HeartbeatMSGModel> msg { set; get; }
        public string remark { set; get; }
    }
}
