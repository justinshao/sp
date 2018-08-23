namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SysRole
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string RecordID { get; set; }

        [Required]
        [StringLength(30)]
        public string RoleName { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int HaveUpdate { get; set; }

        [Required]
        [StringLength(50)]
        public string CPID { get; set; }

        public int DataStatus { get; set; }

        public int IsDefaultRole { get; set; }
    }
}
