namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkGate")]
    public partial class ParkGate
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string GateID { get; set; }

        [StringLength(20)]
        public string GateNo { get; set; }

        [Required]
        [StringLength(20)]
        public string GateName { get; set; }

        [Required]
        [StringLength(50)]
        public string BoxID { get; set; }

        public int IoState { get; set; }

        public int IsTempInOut { get; set; }

        public int IsEnterConfirm { get; set; }

        public int OpenPlateBlurryMatch { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        public int DataStatus { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int HaveUpdate { get; set; }

        public bool? IsNeedCapturePaper { get; set; }

        public bool? PlateNumberAndCard { get; set; }

        public bool? IsWeight { get; set; }
    }
}
