namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WX_Menu
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string MenuName { get; set; }

        [StringLength(200)]
        public string Url { get; set; }

        public int? KeywordId { get; set; }

        public int MenuType { get; set; }

        public int Sort { get; set; }

        public int? MasterID { get; set; }

        public int DataStatus { get; set; }

        public DateTime CreateTime { get; set; }

        [StringLength(50)]
        public string CompanyID { get; set; }

        [StringLength(50)]
        public string MinIprogramAppId { get; set; }

        [StringLength(200)]
        public string MinIprogramPagePath { get; set; }
    }
}
