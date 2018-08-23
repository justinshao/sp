namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkBlacklist")]
    public partial class ParkBlacklist
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string RecordID { get; set; }

        [StringLength(50)]
        public string PKID { get; set; }

        [StringLength(20)]
        public string PlateNumber { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? HaveUpdate { get; set; }

        public int? DataStatus { get; set; }

        public int? Status { get; set; }
    }
}
