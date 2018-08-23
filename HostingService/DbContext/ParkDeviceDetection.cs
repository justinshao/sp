namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkDeviceDetection")]
    public partial class ParkDeviceDetection
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string RecordID { get; set; }

        [StringLength(50)]
        public string DeviceID { get; set; }

        [StringLength(50)]
        public string PKID { get; set; }

        public int? ConnectionState { get; set; }

        public DateTime? DisconnectTime { get; set; }

        public int? DataStatus { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? HaveUpdate { get; set; }
    }
}
