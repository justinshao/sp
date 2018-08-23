namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkArea")]
    public partial class ParkArea
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string AreaID { get; set; }

        [Required]
        [StringLength(25)]
        public string AreaName { get; set; }

        [StringLength(50)]
        public string MasterID { get; set; }

        [Required]
        [StringLength(50)]
        public string PKID { get; set; }

        public int CarbitNum { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int HaveUpdate { get; set; }

        public int NeedToll { get; set; }

        public int CameraWaitTime { get; set; }

        public int TwoCameraWait { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        public int DataStatus { get; set; }
    }
}
