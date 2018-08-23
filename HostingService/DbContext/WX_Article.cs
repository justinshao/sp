namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WX_Article
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [StringLength(200)]
        public string ImagePath { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(200)]
        public string Url { get; set; }

        [StringLength(2000)]
        public string Text { get; set; }

        public int ArticleType { get; set; }

        public int Sort { get; set; }

        [Required]
        [StringLength(50)]
        public string GroupID { get; set; }

        public int DataStatus { get; set; }

        public DateTime CreateTime { get; set; }

        [StringLength(50)]
        public string CompanyID { get; set; }
    }
}
