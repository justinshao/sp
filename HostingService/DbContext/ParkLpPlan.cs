namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkLpPlan")]
    public partial class ParkLpPlan
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string PlanID { get; set; }

        [StringLength(50)]
        public string PKID { get; set; }

        [StringLength(50)]
        public string PlateNumber { get; set; }

        public DateTime? PlanOutTime { get; set; }

        public DateTime? PlanRtnTime { get; set; }

        public DateTime? FactOutTime { get; set; }

        public DateTime? FactRtnTime { get; set; }

        [StringLength(50)]
        public string EmpName { get; set; }

        [StringLength(250)]
        public string Reason { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        public int? PlanState { get; set; }

        [StringLength(50)]
        public string ZB_User { get; set; }

        public DateTime? ZB_Time { get; set; }

        [StringLength(50)]
        public string SH_User { get; set; }

        public DateTime? SH_Time { get; set; }

        public int? SH_State { get; set; }

        public int? DataStatus { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? HaveUpdate { get; set; }
    }
}
