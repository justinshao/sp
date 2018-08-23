namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WX_CarInfo
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string RecordID { get; set; }

        [StringLength(50)]
        public string AccountID { get; set; }

        [StringLength(50)]
        public string PlateID { get; set; }

        [StringLength(20)]
        public string PlateNo { get; set; }

        public int? Status { get; set; }

        [StringLength(20)]
        public string CarMonitor { get; set; }

        public DateTime? BindTime { get; set; }
    }
}
