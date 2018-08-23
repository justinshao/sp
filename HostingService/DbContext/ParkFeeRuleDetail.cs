namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkFeeRuleDetail")]
    public partial class ParkFeeRuleDetail
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string RuleDetailID { get; set; }

        [StringLength(50)]
        public string RuleID { get; set; }

        [StringLength(20)]
        public string StartTime { get; set; }

        [StringLength(20)]
        public string EndTime { get; set; }

        public bool? Supplement { get; set; }

        public int? LoopType { get; set; }

        public decimal? Limit { get; set; }

        public int? FreeTime { get; set; }

        public int? FirstTime { get; set; }

        public decimal? FirstFee { get; set; }

        public int? Loop1PerTime { get; set; }

        public decimal? Loop1PerFee { get; set; }

        public int? Loop2Start { get; set; }

        public int? Loop2PerTime { get; set; }

        public decimal? Loop2PerFee { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? HaveUpdate { get; set; }

        public int? DataStatus { get; set; }
    }
}
