namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WX_Keyword
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Keyword { get; set; }

        public int KeywordType { get; set; }

        public int MatchType { get; set; }

        public int ReplyType { get; set; }

        [StringLength(2000)]
        public string Text { get; set; }

        [StringLength(50)]
        public string ArticleGroupID { get; set; }

        public int DataStatus { get; set; }

        public DateTime CreateTime { get; set; }

        [StringLength(50)]
        public string CompanyID { get; set; }
    }
}
