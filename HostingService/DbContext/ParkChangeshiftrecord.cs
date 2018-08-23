namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkChangeshiftrecord")]
    public partial class ParkChangeshiftrecord
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string RecordID { get; set; }

        [StringLength(50)]
        public string UserID { get; set; }

        public DateTime? StartWorkTime { get; set; }

        public DateTime? EndWorkTime { get; set; }

        [StringLength(50)]
        public string BoxID { get; set; }

        [StringLength(50)]
        public string PKID { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? HaveUpdate { get; set; }

        public int? DataStatus { get; set; }
    }
}
