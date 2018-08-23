namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkSettlement")]
    public partial class ParkSettlement
    {
        [Key]
        [Column(Order = 0)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(36)]
        public string RecordID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(36)]
        public string PKID { get; set; }

        [StringLength(50)]
        public string PKName { get; set; }

        [StringLength(20)]
        public string Priod { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SettleStatus { get; set; }

        public decimal? TotalAmount { get; set; }

        public decimal? HandlingFeeAmount { get; set; }

        public decimal? ReceivableAmount { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? PayTime { get; set; }

        public DateTime? CreateTime { get; set; }

        [StringLength(200)]
        public string Receipt { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(36)]
        public string CreateUser { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(250)]
        public string Remark { get; set; }
    }
}
