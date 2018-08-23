namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SysScope")]
    public partial class SysScope
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string ASID { get; set; }

        [Required]
        [StringLength(50)]
        public string CPID { get; set; }

        [Required]
        [StringLength(50)]
        public string ASName { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int HaveUpdate { get; set; }

        public int DataStatus { get; set; }

        public int IsDefaultScope { get; set; }
    }
}
