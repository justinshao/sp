namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeePlate")]
    public partial class EmployeePlate
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string PlateID { get; set; }

        [Required]
        [StringLength(50)]
        public string EmployeeID { get; set; }

        [Required]
        [StringLength(20)]
        public string PlateNo { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int HaveUpdate { get; set; }

        public int DataStatus { get; set; }

        public int Color { get; set; }

        [StringLength(50)]
        public string CarBrand { get; set; }
    }
}
