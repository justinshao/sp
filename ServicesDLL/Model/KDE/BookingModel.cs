using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesDLL.Model.KDE
{
    public class BookingModel
    {
        public string PlateId { set; get; }
        public string StartTime { set; get; }
        public string EndTime { set; get; }
        public int Charge { set; get; }
        public string ActionFlag { set; get; }
        public string OrderNo { set; get; }
        public string Timestamp { set; get; }
        public string MessageId { set; get; }
    }
}
