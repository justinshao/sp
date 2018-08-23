namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WX_InteractionInfo
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string ReplyID { get; set; }

        [Required]
        [StringLength(50)]
        public string OpenID { get; set; }

        public int MsgType { get; set; }

        [Required]
        [StringLength(2000)]
        public string InteractionContent { get; set; }

        public DateTime CreateTime { get; set; }

        [StringLength(50)]
        public string CompanyID { get; set; }
    }
}
