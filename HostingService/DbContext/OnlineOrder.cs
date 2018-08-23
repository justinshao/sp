namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OnlineOrder")]
    public partial class OnlineOrder
    {
        [Key]
        public decimal OrderID { get; set; }

        [Required]
        [StringLength(50)]
        public string PKID { get; set; }

        [StringLength(50)]
        public string PKName { get; set; }

        [StringLength(50)]
        public string InOutID { get; set; }

        [StringLength(50)]
        public string PlateNo { get; set; }

        public DateTime? EntranceTime { get; set; }

        public DateTime? ExitTime { get; set; }

        public decimal? Amount { get; set; }

        public int PaymentChannel { get; set; }

        public int? PayBank { get; set; }

        [Required]
        [StringLength(50)]
        public string PayAccount { get; set; }

        [Required]
        [StringLength(50)]
        public string Payer { get; set; }

        public int PayeeChannel { get; set; }

        public int? PayeeBank { get; set; }

        [StringLength(50)]
        public string PayeeAccount { get; set; }

        [StringLength(50)]
        public string PayeeUser { get; set; }

        [StringLength(50)]
        public string PayDetailID { get; set; }

        [StringLength(50)]
        public string SerialNumber { get; set; }

        [StringLength(50)]
        public string PrepayId { get; set; }

        public int? MonthNum { get; set; }

        [StringLength(50)]
        public string AccountID { get; set; }

        [StringLength(50)]
        public string CardId { get; set; }

        public int? SyncResultTimes { get; set; }

        public DateTime? LastSyncResultTime { get; set; }

        [StringLength(50)]
        public string ParkCardNo { get; set; }

        public decimal? Balance { get; set; }

        [StringLength(50)]
        public string RefundOrderId { get; set; }

        [StringLength(100)]
        public string Remark { get; set; }

        public int OrderType { get; set; }

        public int Status { get; set; }

        public DateTime OrderTime { get; set; }

        public DateTime? RealPayTime { get; set; }

        public DateTime? BookingStartTime { get; set; }

        public DateTime? BookingEndTime { get; set; }

        [StringLength(50)]
        public string BookingAreaID { get; set; }

        [StringLength(50)]
        public string BookingBitNo { get; set; }

        [StringLength(50)]
        public string CompanyID { get; set; }

        public int? OrderSource { get; set; }

        public int? ExternalPKID { get; set; }
    }
}
