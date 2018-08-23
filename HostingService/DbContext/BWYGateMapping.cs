namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BWYGateMapping")]
    public partial class BWYGateMapping
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string RecordID { get; set; }

        [StringLength(50)]
        public string ParkingID { get; set; }

        [StringLength(50)]
        public string ParkingName { get; set; }

        [StringLength(50)]
        public string GateID { get; set; }

        [StringLength(50)]
        public string GateName { get; set; }

        public DateTime? CreateTime { get; set; }

        public int DataStatus { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? HaveUpdate { get; set; }
    }
}
