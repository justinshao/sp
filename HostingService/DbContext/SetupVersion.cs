namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SetupVersion")]
    public partial class SetupVersion
    {
        public int ID { get; set; }

        [Required]
        [StringLength(36)]
        public string SetupVersionID { get; set; }

        public short VersionCategory { get; set; }

        public short VersionStatus { get; set; }

        [Required]
        [StringLength(50)]
        public string VersionCode { get; set; }

        public int VersionType { get; set; }

        public int? FileCount { get; set; }

        [StringLength(2000)]
        public string Villages { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        [StringLength(36)]
        public string PubUser { get; set; }

        public DateTime? Pubdate { get; set; }
    }
}
