namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkDerateConfig")]
    public partial class ParkDerateConfig
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string RecordID { get; set; }

        [Required]
        [StringLength(50)]
        public string PKID { get; set; }

        public decimal ConsumeStartAmount { get; set; }

        public decimal ConsumeEndAmount { get; set; }

        public int DerateType { get; set; }

        public decimal DerateValue { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int HaveUpdate { get; set; }

        public int DataStatus { get; set; }
    }
}
