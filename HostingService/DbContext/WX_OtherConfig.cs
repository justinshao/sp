namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WX_OtherConfig
    {
        public int ID { get; set; }

        public int ConfigType { get; set; }

        [Required]
        [StringLength(400)]
        public string ConfigValue { get; set; }

        [StringLength(50)]
        public string CompanyID { get; set; }
    }
}
