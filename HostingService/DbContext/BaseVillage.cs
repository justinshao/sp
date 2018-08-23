namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BaseVillage")]
    public partial class BaseVillage
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string VID { get; set; }

        [StringLength(10)]
        public string VNo { get; set; }

        [Required]
        [StringLength(50)]
        public string VName { get; set; }

        [Required]
        [StringLength(50)]
        public string CPID { get; set; }

        [StringLength(50)]
        public string LinkMan { get; set; }

        [StringLength(50)]
        public string Mobile { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Coordinate { get; set; }

        [Required]
        [StringLength(50)]
        public string ProxyNo { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int HaveUpdate { get; set; }

        public int DataStatus { get; set; }

        public int? IsOnLine { get; set; }

        public bool? IsBoxWatch { get; set; }
    }
}
