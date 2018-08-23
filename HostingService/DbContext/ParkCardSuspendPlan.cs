namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkCardSuspendPlan")]
    public partial class ParkCardSuspendPlan
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string RecordId { get; set; }

        [Required]
        [StringLength(50)]
        public string GrantID { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int HaveUpdate { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int DataStatus { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
