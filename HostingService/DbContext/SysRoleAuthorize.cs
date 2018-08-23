namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SysRoleAuthorize")]
    public partial class SysRoleAuthorize
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string RecordID { get; set; }

        [Required]
        [StringLength(50)]
        public string ModuleID { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleID { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int HaveUpdate { get; set; }

        public int DataStatus { get; set; }

        [StringLength(50)]
        public string ParentID { get; set; }
    }
}
