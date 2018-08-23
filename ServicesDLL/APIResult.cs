using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;

namespace ServicesDLL
{
    public class APIResult
    {
        public string result { set; get; }
        public object data { set; get; }
        public int dataSum { set; get; }
        public int PageCount = 50;
        public int PageIndex { set; get; }
    }
}
