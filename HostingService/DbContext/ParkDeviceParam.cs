namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkDeviceParam")]
    public partial class ParkDeviceParam
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string RecordID { get; set; }

        [StringLength(50)]
        public string DeviceID { get; set; }

        public int? VipMode { get; set; }

        public int? TempMode { get; set; }

        public int? NetOffMode { get; set; }

        public int? VipDevMultIn { get; set; }

        public int? PloicFree { get; set; }

        public int? VipDutyDay { get; set; }

        public int? OverDutyYorN { get; set; }

        public int? OverDutyDay { get; set; }

        public int? SysID { get; set; }

        public int? DevID { get; set; }

        public int? SysInDev { get; set; }

        public int? SysOutDev { get; set; }

        public int? SysParkNumber { get; set; }

        public int? DevInorOut { get; set; }

        public int? SwipeInterval { get; set; }

        public int? UnKonwCardType { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int HaveUpdate { get; set; }

        public int DataStatus { get; set; }

        public int? LEDNumber { get; set; }
    }
}
