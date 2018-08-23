namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkSeller")]
    public partial class ParkSeller
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string SellerID { get; set; }

        [Required]
        [StringLength(20)]
        public string SellerNo { get; set; }

        [Required]
        [StringLength(25)]
        public string SellerName { get; set; }

        [Required]
        [StringLength(50)]
        public string PWD { get; set; }

        [Required]
        [StringLength(50)]
        public string VID { get; set; }

        [StringLength(250)]
        public string Addr { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int HaveUpdate { get; set; }

        public int DataStatus { get; set; }

        public int Creditline { get; set; }

        public decimal Balance { get; set; }

        [StringLength(128)]
        public string PPSellerID { get; set; }
    }
}
