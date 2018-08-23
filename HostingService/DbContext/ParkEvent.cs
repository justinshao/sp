namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkEvent")]
    public partial class ParkEvent
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string RecordID { get; set; }

        [StringLength(50)]
        public string CardNo { get; set; }

        [StringLength(50)]
        public string CardNum { get; set; }

        [StringLength(20)]
        public string EmployeeName { get; set; }

        [StringLength(50)]
        public string PlateNumber { get; set; }

        public int? PlateColor { get; set; }

        [StringLength(50)]
        public string CarTypeID { get; set; }

        [StringLength(50)]
        public string CarModelID { get; set; }

        public DateTime? RecTime { get; set; }

        [StringLength(50)]
        public string GateID { get; set; }

        [StringLength(50)]
        public string OperatorID { get; set; }

        [StringLength(250)]
        public string PictureName { get; set; }

        public int? EventID { get; set; }

        public int? IOState { get; set; }

        [StringLength(50)]
        public string IORecordID { get; set; }

        [StringLength(50)]
        public string ParkingID { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? HaveUpdate { get; set; }

        public int? DataStatus { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        public bool? IsScanCode { get; set; }

        public int? IsOffline { get; set; }

        public int? reportState { get; set; }

        [StringLength(50)]
        public string CertName { get; set; }

        [StringLength(50)]
        public string CertNo { get; set; }

        [StringLength(10)]
        public string Sex { get; set; }

        [StringLength(50)]
        public string Nation { get; set; }

        public DateTime? BirthDate { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [StringLength(250)]
        public string CertificateImage { get; set; }
    }
}
