namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BaseParkinfo")]
    public partial class BaseParkinfo
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string PKID { get; set; }

        [StringLength(50)]
        public string PKNo { get; set; }

        [Required]
        [StringLength(25)]
        public string PKName { get; set; }

        public int CarBitNum { get; set; }

        public int CarBitNumLeft { get; set; }

        public int CarBitNumFixed { get; set; }

        public int SpaceBitNum { get; set; }

        public int CenterTime { get; set; }

        public int AllowLoseDisplay { get; set; }

        [StringLength(20)]
        public string LinkMan { get; set; }

        [StringLength(20)]
        public string Mobile { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Coordinate { get; set; }

        public int MobilePay { get; set; }

        public int MobileLock { get; set; }

        public int IsParkingSpace { get; set; }

        public int IsReverseSeekingVehicle { get; set; }

        [StringLength(250)]
        public string FeeRemark { get; set; }

        public int OnLine { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int HaveUpdate { get; set; }

        public int CityID { get; set; }

        [Required]
        [StringLength(50)]
        public string VID { get; set; }

        public int DataStatus { get; set; }

        public int DataSaveDays { get; set; }

        public int PictureSaveDays { get; set; }

        public int IsOnLineGathe { get; set; }

        public int IsLine { get; set; }

        public int NeedFee { get; set; }

        public int ExpiredAdvanceRemindDay { get; set; }

        [StringLength(3)]
        public string DefaultPlate { get; set; }

        public bool? PoliceFree { get; set; }

        public decimal? OnlineDiscount { get; set; }

        public bool? IsOnlineDiscount { get; set; }

        public bool? IsNoPlateConfirm { get; set; }

        public bool? UnconfirmedCalculation { get; set; }

        public bool? SupportAutoRefund { get; set; }

        public decimal? HandlingFee { get; set; }

        public decimal? MaxAmountOfCash { get; set; }

        public bool? OuterringCharge { get; set; }

        public decimal? MinAmountOfCash { get; set; }
    }
}
