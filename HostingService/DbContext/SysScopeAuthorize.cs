namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SysScopeAuthorize")]
    public partial class SysScopeAuthorize
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string ASID { get; set; }

        [Required]
        [StringLength(50)]
        public string ASDID { get; set; }

        [Required]
        [StringLength(50)]
        public string TagID { get; set; }

        public int ASType { get; set; }

        [Required]
        [StringLength(50)]
        public string CPID { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int HaveUpdate { get; set; }

        public int DataStatus { get; set; }
    }
}
