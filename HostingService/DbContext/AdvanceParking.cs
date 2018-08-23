namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AdvanceParking")]
    public partial class AdvanceParking
    {
        public int ID { get; set; }

        public decimal OrderId { get; set; }

        [Required]
        [StringLength(20)]
        public string PlateNo { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public decimal Amount { get; set; }

        [Required]
        [StringLength(50)]
        public string WxOpenId { get; set; }

        public int OrderState { get; set; }

        public DateTime? PayTime { get; set; }

        [StringLength(50)]
        public string PrepayId { get; set; }

        [StringLength(50)]
        public string SerialNumber { get; set; }

        public DateTime CreateTime { get; set; }

        [StringLength(50)]
        public string CompanyID { get; set; }
    }
}
