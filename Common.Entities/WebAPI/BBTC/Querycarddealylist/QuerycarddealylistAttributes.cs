using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.WebAPI.BBTC
{
    public class QuerycarddealylistAttributes 
    {
        public string cardId { set; get; }
        public string beginDate { set; get; }
        public string endDate { set; get; }
        public string pageSize { set; get; }
        public string pageIndex { set; get; }
    }
}
