namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkFeeRule")]
    public partial class ParkFeeRule
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string FeeRuleID { get; set; }

        [StringLength(50)]
        public string RuleName { get; set; }

        public int? FeeType { get; set; }

        [StringLength(50)]
        public string CarTypeID { get; set; }

        [StringLength(50)]
        public string CarModelID { get; set; }

        [StringLength(50)]
        public string AreaID { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? HaveUpdate { get; set; }

        public int? DataStatus { get; set; }

        [Column(TypeName = "ntext")]
        public string RuleText { get; set; }

        public bool? IsOffline { get; set; }
    }
}
