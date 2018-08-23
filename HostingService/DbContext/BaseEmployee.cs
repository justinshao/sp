namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BaseEmployee")]
    public partial class BaseEmployee
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string EmployeeID { get; set; }

        [Required]
        [StringLength(20)]
        public string EmployeeName { get; set; }

        [StringLength(2)]
        public string Sex { get; set; }

        public int CertifType { get; set; }

        [StringLength(25)]
        public string CertifNo { get; set; }

        [Required]
        [StringLength(20)]
        public string MobilePhone { get; set; }

        [StringLength(20)]
        public string HomePhone { get; set; }

        [StringLength(20)]
        public string Email { get; set; }

        [StringLength(250)]
        public string FamilyAddr { get; set; }

        public DateTime RegTime { get; set; }

        public int EmployeeType { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int HaveUpdate { get; set; }

        public int DataStatus { get; set; }

        [StringLength(20)]
        public string AreaName { get; set; }

        [StringLength(50)]
        public string DeptID { get; set; }

        [Required]
        [StringLength(50)]
        public string VID { get; set; }
    }
}
