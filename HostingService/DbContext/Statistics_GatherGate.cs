namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Statistics_GatherGate
    {
        public int ID { get; set; }

        [Required]
        [StringLength(36)]
        public string StatisticsGatherID { get; set; }

        [Required]
        [StringLength(36)]
        public string ParkingID { get; set; }

        [Required]
        [StringLength(100)]
        public string ParkingName { get; set; }

        [Required]
        [StringLength(36)]
        public string BoxID { get; set; }

        [Required]
        [StringLength(100)]
        public string BoxName { get; set; }

        [Required]
        [StringLength(36)]
        public string GateID { get; set; }

        [Required]
        [StringLength(100)]
        public string GateName { get; set; }

        public DateTime GatherTime { get; set; }

        public decimal? Receivable_Amount { get; set; }

        public decimal? Real_Amount { get; set; }

        public decimal? Diff_Amount { get; set; }

        public decimal? Cash_Amount { get; set; }

        public int? Cash_Count { get; set; }

        public decimal? Temp_Amount { get; set; }

        public int? Temp_Count { get; set; }

        public decimal? OnLineTemp_Amount { get; set; }

        public int? OnLineTemp_Count { get; set; }

        public decimal? StordCard_Amount { get; set; }

        public int? StordCard_Count { get; set; }

        public decimal? OnLine_Amount { get; set; }

        public int? OnLine_Count { get; set; }

        public decimal? Discount_Amount { get; set; }

        public int? Discount_Count { get; set; }

        public int? ReleaseType_Normal { get; set; }

        public int? ReleaseType_Charge { get; set; }

        public int? ReleaseType_Free { get; set; }

        public int? ReleaseType_Catch { get; set; }

        public int? VIPExtend_Count { get; set; }

        public int? Entrance_Count { get; set; }

        public int? Exit_Count { get; set; }

        public int? VIPCard { get; set; }

        public int? StordCard { get; set; }

        public int? MonthCard { get; set; }

        public int? JobCard { get; set; }

        public int? TempCard { get; set; }

        public int? OnLineMonthCardExtend_Count { get; set; }

        public decimal? OnLineMonthCardExtend_Amount { get; set; }

        public int? MonthCardExtend_Count { get; set; }

        public decimal? MonthCardExtend_Amount { get; set; }

        public int? OnLineStordCard_Count { get; set; }

        public decimal? OnLineStordCard_Amount { get; set; }

        public int? StordCardRecharge_Count { get; set; }

        public decimal? StordCardRecharge_Amount { get; set; }

        public decimal? CashDiscount_Amount { get; set; }

        public int? CashDiscount_Count { get; set; }

        public decimal? OnLineDiscount_Amount { get; set; }

        public int? OnLineDiscount_Count { get; set; }

        public int? HaveUpdate { get; set; }

        public DateTime? LastUpdateTime { get; set; }
    }
}
