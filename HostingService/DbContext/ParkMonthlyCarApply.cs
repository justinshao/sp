namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkMonthlyCarApply")]
    public partial class ParkMonthlyCarApply
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string RecordID { get; set; }

        [Required]
        [StringLength(50)]
        public string AccountID { get; set; }

        [Required]
        [StringLength(50)]
        public string PKID { get; set; }

        [Required]
        [StringLength(50)]
        public string CarTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string CarModelID { get; set; }

        [Required]
        [StringLength(50)]
        public string ApplyName { get; set; }

        [Required]
        [StringLength(50)]
        public string ApplyMoblie { get; set; }

        [Required]
        [StringLength(50)]
        public string PlateNo { get; set; }

        [StringLength(50)]
        public string PKLot { get; set; }

        [StringLength(100)]
        public string FamilyAddress { get; set; }

        [StringLength(200)]
        public string ApplyRemark { get; set; }

        public int ApplyStatus { get; set; }

        [StringLength(200)]
        public string AuditRemark { get; set; }

        public DateTime ApplyDateTime { get; set; }
    }
}
