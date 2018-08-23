namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkOrder")]
    public partial class ParkOrder
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string RecordID { get; set; }

        [StringLength(20)]
        public string OrderNo { get; set; }

        [StringLength(50)]
        public string TagID { get; set; }

        public int? OrderType { get; set; }

        public int? PayWay { get; set; }

        public decimal? Amount { get; set; }

        public decimal? UnPayAmount { get; set; }

        public decimal? PayAmount { get; set; }

        public decimal? DiscountAmount { get; set; }

        [StringLength(500)]
        public string CarderateID { get; set; }

        public int? Status { get; set; }

        public int? OrderSource { get; set; }

        public DateTime? OrderTime { get; set; }

        public DateTime? PayTime { get; set; }

        public DateTime? OldUserBegin { get; set; }

        public DateTime? OldUserulDate { get; set; }

        public DateTime? NewUserBegin { get; set; }

        public DateTime? NewUsefulDate { get; set; }

        public decimal? OldMoney { get; set; }

        public decimal? NewMoney { get; set; }

        [StringLength(50)]
        public string PKID { get; set; }

        [StringLength(50)]
        public string UserID { get; set; }

        [StringLength(50)]
        public string OnlineUserID { get; set; }

        [StringLength(50)]
        public string OnlineOrderNo { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? HaveUpdate { get; set; }

        public int? DataStatus { get; set; }

        public DateTime? CashTime { get; set; }

        public decimal? CashMoney { get; set; }

        [StringLength(50)]
        public string FeeRuleID { get; set; }
    }
}
