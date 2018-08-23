namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkGrant")]
    public partial class ParkGrant
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string GID { get; set; }

        [Required]
        [StringLength(50)]
        public string CardID { get; set; }

        [Required]
        [StringLength(50)]
        public string PKID { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        [StringLength(50)]
        public string CarTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string CarModelID { get; set; }

        [StringLength(25)]
        public string PKLot { get; set; }

        [Required]
        [StringLength(50)]
        public string PlateID { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int HaveUpdate { get; set; }

        public int ComdState { get; set; }

        [StringLength(250)]
        public string AreaIDS { get; set; }

        [StringLength(250)]
        public string GateID { get; set; }

        public int State { get; set; }

        public int DataStatus { get; set; }

        public int? PKLotNum { get; set; }
    }
}
