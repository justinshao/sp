namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkCarDerate")]
    public partial class ParkCarDerate
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string CarDerateID { get; set; }

        [StringLength(50)]
        public string CarDerateNo { get; set; }

        [Required]
        [StringLength(50)]
        public string DerateID { get; set; }

        public int FreeTime { get; set; }

        public decimal FreeMoney { get; set; }

        [Required]
        [StringLength(50)]
        public string PlateNumber { get; set; }

        [StringLength(50)]
        public string CardNo { get; set; }

        [StringLength(50)]
        public string IORecordID { get; set; }

        public int Status { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ExpiryTime { get; set; }

        [Required]
        [StringLength(50)]
        public string PKID { get; set; }

        [Required]
        [StringLength(50)]
        public string AreaID { get; set; }

        public int HaveUpdate { get; set; }

        public int DataStatus { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int? Enforce { get; set; }

        [StringLength(128)]
        public string Reason { get; set; }

        [StringLength(128)]
        public string Identity { get; set; }
    }
}
