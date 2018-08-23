namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkCarModel")]
    public partial class ParkCarModel
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string CarModelID { get; set; }

        [Required]
        [StringLength(20)]
        public string CarModelName { get; set; }

        [Required]
        [StringLength(50)]
        public string PKID { get; set; }

        public int IsDefault { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int HaveUpdate { get; set; }

        public int DataStatus { get; set; }

        public decimal? MaxUseMoney { get; set; }

        public bool? IsNaturalDay { get; set; }

        [StringLength(10)]
        public string PlateColor { get; set; }

        public decimal? DayMaxMoney { get; set; }

        public decimal? NightMaxMoney { get; set; }

        public DateTime? DayStartTime { get; set; }

        public DateTime? DayEndTime { get; set; }

        public DateTime? NightStartTime { get; set; }

        public DateTime? NightEndTime { get; set; }

        public int? NaturalTime { get; set; }
    }
}
