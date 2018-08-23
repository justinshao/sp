namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkInterim")]
    public partial class ParkInterim
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string RecordID { get; set; }

        [StringLength(50)]
        public string IORecordID { get; set; }

        public DateTime? StartInterimTime { get; set; }

        public DateTime? EndInterimTime { get; set; }

        public int? HaveUpdate { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? DataStatus { get; set; }

        [StringLength(50)]
        public string Remark { get; set; }
    }
}
