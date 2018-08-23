namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkDerateQRcode")]
    public partial class ParkDerateQRcode
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string RecordID { get; set; }

        [Required]
        [StringLength(50)]
        public string DerateID { get; set; }

        public decimal? DerateValue { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int DataSource { get; set; }

        [StringLength(50)]
        public string OperatorId { get; set; }

        public DateTime CreateTime { get; set; }

        [StringLength(100)]
        public string Remark { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int HaveUpdate { get; set; }

        public int DataStatus { get; set; }

        public int CanUseTimes { get; set; }

        public int AlreadyUseTimes { get; set; }
    }
}
