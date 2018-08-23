namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkVisitor")]
    public partial class ParkVisitor
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string RecordID { get; set; }

        [StringLength(50)]
        public string VisitorID { get; set; }

        [StringLength(50)]
        public string PKID { get; set; }

        [StringLength(50)]
        public string VID { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? HaveUpdate { get; set; }

        public int? DataStatus { get; set; }

        public int? AlreadyVisitorCount { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }
    }
}
