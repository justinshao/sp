namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkSettlementApprovalFlow")]
    public partial class ParkSettlementApprovalFlow
    {
        [Key]
        [Column(Order = 0)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string FlowName { get; set; }

        [StringLength(36)]
        public string PKID { get; set; }

        [StringLength(36)]
        public string FlowUser { get; set; }

        [StringLength(100)]
        public string Remark { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FlowID { get; set; }
    }
}
