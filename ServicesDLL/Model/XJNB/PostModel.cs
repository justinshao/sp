using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.XJNB
{
    public class PostModel
    {
        public string name { set; get; }
        public string version { set; get; }
        public string app_key { set; get; }
        public string data { set; get; }
        public string format { set; get; }
        public string timestamp { set; get; }
        public string sign { set; get; }
    }
}
