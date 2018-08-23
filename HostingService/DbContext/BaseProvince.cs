namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BaseProvince")]
    public partial class BaseProvince
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProvinceID { get; set; }

        [Required]
        [StringLength(20)]
        public string ProvinceName { get; set; }

        public int OrderIndex { get; set; }

        public double? Longitude { get; set; }

        public double? Lititute { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }
    }
}
