namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkGateIOTime")]
    public partial class ParkGateIOTime
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string RecordID { get; set; }

        [StringLength(50)]
        public string GateID { get; set; }

        public int? RuleType { get; set; }

        public int? WeekIndex { get; set; }

        public DateTime? RuleDate { get; set; }

        [StringLength(10)]
        public string StartTime { get; set; }

        [StringLength(10)]
        public string EndTime { get; set; }

        public int? InOutState { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? HaveUpdate { get; set; }

        public int? DataStatus { get; set; }
    }
}
