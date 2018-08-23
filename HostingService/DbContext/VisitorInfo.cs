namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VisitorInfo")]
    public partial class VisitorInfo
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string RecordID { get; set; }

        [StringLength(50)]
        public string PlateNumber { get; set; }

        [StringLength(20)]
        public string VisitorMobilePhone { get; set; }

        public int? VisitorCount { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(50)]
        public string AccountID { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? VisitorState { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? HaveUpdate { get; set; }

        public int? DataStatus { get; set; }

        [StringLength(50)]
        public string VID { get; set; }

        public int? LookSate { get; set; }

        public int? IsExamine { get; set; }

        [StringLength(50)]
        public string VisitorName { get; set; }

        [StringLength(2)]
        public string VisitorSex { get; set; }

        public DateTime? Birthday { get; set; }

        public int? CertifType { get; set; }

        [StringLength(50)]
        public string CertifNo { get; set; }

        [StringLength(250)]
        public string CertifAddr { get; set; }

        [StringLength(50)]
        public string EmployeeID { get; set; }

        [StringLength(20)]
        public string CardNo { get; set; }

        [StringLength(50)]
        public string OperatorID { get; set; }

        [StringLength(50)]
        public string VisitorCompany { get; set; }

        [StringLength(250)]
        public string VistorPhoto { get; set; }

        [StringLength(250)]
        public string CertifPhoto { get; set; }

        public int? VisitorSource { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        [StringLength(50)]
        public string CarModelID { get; set; }
    }
}
