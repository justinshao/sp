namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LoginLog")]
    public partial class LoginLog
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string UserAccount { get; set; }

        [StringLength(20)]
        public string LoginIP { get; set; }

        public DateTime LoginTime { get; set; }

        public DateTime? LogoutTime { get; set; }

        public int LogFrom { get; set; }

        [StringLength(400)]
        public string Remark { get; set; }
    }
}
