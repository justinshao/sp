namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkReserveBit")]
    public partial class ParkReserveBit
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string RecordID { get; set; }

        [Required]
        [StringLength(50)]
        public string AccountID { get; set; }

        [Required]
        [StringLength(50)]
        public string PKID { get; set; }

        [StringLength(20)]
        public string BitNo { get; set; }

        [StringLength(50)]
        public string PlateNumber { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? CreateTime { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        public int? Status { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? HaveUpdate { get; set; }

        public int? DataStatus { get; set; }
    }
}
