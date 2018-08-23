namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkDevice")]
    public partial class ParkDevice
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string DeviceID { get; set; }

        [Required]
        [StringLength(50)]
        public string GateID { get; set; }

        public int DeviceType { get; set; }

        public int PortType { get; set; }

        public int? Baudrate { get; set; }

        [StringLength(50)]
        public string SerialPort { get; set; }

        [StringLength(20)]
        public string IpAddr { get; set; }

        public int? IpPort { get; set; }

        [StringLength(15)]
        public string UserName { get; set; }

        [StringLength(10)]
        public string UserPwd { get; set; }

        public int? NetID { get; set; }

        public int? LedNum { get; set; }

        public int DataStatus { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int HaveUpdate { get; set; }

        [StringLength(50)]
        public string DeviceNo { get; set; }

        public int? OfflinePort { get; set; }

        public bool? IsCapture { get; set; }

        public bool? IsSVoice { get; set; }

        public bool? IsCarBit { get; set; }

        public bool? IsContestDev { get; set; }

        public int? ControllerType { get; set; }

        public int? DisplayMode { get; set; }

        public bool? IsMonitor { get; set; }
    }
}
