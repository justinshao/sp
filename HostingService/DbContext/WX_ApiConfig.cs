namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WX_ApiConfig
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Domain { get; set; }

        [Required]
        [StringLength(50)]
        public string ServerIP { get; set; }

        [Required]
        [StringLength(50)]
        public string SystemName { get; set; }

        [Required]
        [StringLength(200)]
        public string SystemLogo { get; set; }

        [Required]
        [StringLength(50)]
        public string AppId { get; set; }

        [Required]
        [StringLength(50)]
        public string AppSecret { get; set; }

        [Required]
        [StringLength(50)]
        public string Token { get; set; }

        [StringLength(50)]
        public string PartnerKey { get; set; }

        [StringLength(50)]
        public string PartnerId { get; set; }

        [StringLength(200)]
        public string CertPath { get; set; }

        [StringLength(50)]
        public string CertPwd { get; set; }

        [StringLength(50)]
        public string CompanyID { get; set; }

        public bool Status { get; set; }

        public bool SupportSuperiorPay { get; set; }
    }
}
