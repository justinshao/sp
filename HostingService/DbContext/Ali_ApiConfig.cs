namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ali_ApiConfig
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string RecordId { get; set; }

        [Required]
        [StringLength(100)]
        public string SystemName { get; set; }

        [Required]
        [StringLength(200)]
        public string SystemDomain { get; set; }

        [Required]
        [StringLength(50)]
        public string AppId { get; set; }

        [Required]
        [StringLength(4096)]
        public string PublicKey { get; set; }

        [Required]
        [StringLength(4096)]
        public string PrivateKey { get; set; }

        [Required]
        [StringLength(100)]
        public string PayeeAccount { get; set; }

        [StringLength(50)]
        public string CompanyID { get; set; }

        public bool Status { get; set; }

        public bool SupportSuperiorPay { get; set; }

        public int? AliPaySignType { get; set; }
    }
}
