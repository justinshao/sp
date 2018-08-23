namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkDerate")]
    public partial class ParkDerate
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string DerateID { get; set; }

        [Required]
        [StringLength(50)]
        public string SellerID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int DerateSwparate { get; set; }

        public int DerateType { get; set; }

        public decimal? DerateMoney { get; set; }

        public int? FreeTime { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [StringLength(50)]
        public string FeeRuleID { get; set; }

        public int HaveUpdate { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int DataStatus { get; set; }

        public int? MaxTimesCycle { get; set; }

        public int? MaxTimes { get; set; }

        public int? ValidityTime { get; set; }
    }
}
