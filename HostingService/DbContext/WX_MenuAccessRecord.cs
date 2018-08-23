namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WX_MenuAccessRecord
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string MenuName { get; set; }

        [Required]
        [StringLength(50)]
        public string OpenID { get; set; }

        public DateTime AccessTime { get; set; }

        [StringLength(50)]
        public string CompanyID { get; set; }
    }
}
