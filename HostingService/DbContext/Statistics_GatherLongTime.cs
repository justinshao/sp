namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Statistics_GatherLongTime
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string RecordID { get; set; }

        [Required]
        [StringLength(36)]
        public string ParkingID { get; set; }

        public DateTime GatherTime { get; set; }

        public int LTime { get; set; }

        public int Times { get; set; }

        public int? HaveUpdate { get; set; }

        public DateTime? LastUpdateTime { get; set; }
    }
}
