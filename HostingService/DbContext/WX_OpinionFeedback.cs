namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WX_OpinionFeedback
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string OpenId { get; set; }

        [Required]
        [StringLength(1000)]
        public string FeedbackContent { get; set; }

        public DateTime CreateTime { get; set; }

        [StringLength(50)]
        public string CompanyID { get; set; }
    }
}
