namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkBox")]
    public partial class ParkBox
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string BoxID { get; set; }

        [StringLength(25)]
        public string BoxNo { get; set; }

        [Required]
        [StringLength(25)]
        public string BoxName { get; set; }

        [StringLength(20)]
        public string ComputerIP { get; set; }

        [Required]
        [StringLength(50)]
        public string AreaID { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        public int IsCenterPayment { get; set; }

        public int DataStatus { get; set; }

        public int HaveUpdate { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public bool? IsOnLine { get; set; }

        [StringLength(50)]
        public string ProxyNo { get; set; }
    }
}
