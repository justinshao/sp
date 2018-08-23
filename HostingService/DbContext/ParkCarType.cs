namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkCarType")]
    public partial class ParkCarType
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string CarTypeID { get; set; }

        [Required]
        [StringLength(20)]
        public string CarTypeName { get; set; }

        [Required]
        [StringLength(50)]
        public string PKID { get; set; }

        public int BaseTypeID { get; set; }

        public int RepeatIn { get; set; }

        public int RepeatOut { get; set; }

        public int AffirmIn { get; set; }

        public int AffirmOut { get; set; }

        public DateTime? InBeginTime { get; set; }

        public DateTime? InEdnTime { get; set; }

        public decimal MaxUseMoney { get; set; }

        public int AllowLose { get; set; }

        public int LpDistinguish { get; set; }

        public int InOutEditCar { get; set; }

        public int InOutTime { get; set; }

        public int CarNoLike { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int HaveUpdate { get; set; }

        public int IsAllowOnlIne { get; set; }

        public decimal Amount { get; set; }

        public int MaxMonth { get; set; }

        public decimal MaxValue { get; set; }

        public int DataStatus { get; set; }

        public int OverdueToTemp { get; set; }

        public int LotOccupy { get; set; }

        public decimal Deposit { get; set; }

        public int MonthCardExpiredEnterDay { get; set; }

        [StringLength(10)]
        public string AffirmBegin { get; set; }

        [StringLength(10)]
        public string AffirmEnd { get; set; }

        public bool? IsNeedCapturePaper { get; set; }

        public bool? IsNeedAuthentication { get; set; }

        public bool? IsDispatch { get; set; }

        public int? OnlineUnit { get; set; }

        public bool IsIgnoreHZ { get; set; }
    }
}
