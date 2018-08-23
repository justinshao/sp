namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BasePassRemark")]
    public partial class BasePassRemark
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string RecordID { get; set; }

        public int? PassType { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        [StringLength(50)]
        public string PKID { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? HaveUpdate { get; set; }

        public int? DataStatus { get; set; }
    }
}
