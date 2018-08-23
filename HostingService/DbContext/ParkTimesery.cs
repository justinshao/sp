namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParkTimesery
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string TimeseriesID { get; set; }

        [StringLength(50)]
        public string IORecordID { get; set; }

        [StringLength(250)]
        public string EnterImage { get; set; }

        [StringLength(250)]
        public string ExitImage { get; set; }

        [StringLength(50)]
        public string ParkingID { get; set; }

        public DateTime? EnterTime { get; set; }

        public DateTime? ExitTime { get; set; }

        [StringLength(50)]
        public string ExitGateID { get; set; }

        [StringLength(50)]
        public string EnterGateID { get; set; }

        public bool? IsExit { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? HaveUpdate { get; set; }

        public int? DataStatus { get; set; }

        public int? ReleaseType { get; set; }

        [StringLength(50)]
        public string RecordID { get; set; }
    }
}
