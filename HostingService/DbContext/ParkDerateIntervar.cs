namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkDerateIntervar")]
    public partial class ParkDerateIntervar
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string DerateIntervarID { get; set; }

        [Required]
        [StringLength(50)]
        public string DerateID { get; set; }

        public int? FreeTime { get; set; }

        public decimal? Monetry { get; set; }

        public int HaveUpdate { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int DataStatus { get; set; }
    }
}
