namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WX_Account
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string AccountID { get; set; }

        [StringLength(50)]
        public string AccountName { get; set; }

        public int? AccountModel { get; set; }

        [StringLength(50)]
        public string TradePWD { get; set; }

        [StringLength(2)]
        public string Sex { get; set; }

        [StringLength(20)]
        public string Email { get; set; }

        [StringLength(20)]
        public string MobilePhone { get; set; }

        public int? Status { get; set; }

        public DateTime? RegTime { get; set; }

        public int? CityID { get; set; }

        public bool? OpenAnswerPhone { get; set; }

        public bool? IsAutoLock { get; set; }

        [StringLength(50)]
        public string CompanyID { get; set; }
    }
}
