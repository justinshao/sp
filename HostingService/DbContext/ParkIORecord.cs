namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkIORecord")]
    public partial class ParkIORecord
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string RecordID { get; set; }

        [StringLength(50)]
        public string CardID { get; set; }

        [StringLength(50)]
        public string PlateNumber { get; set; }

        [StringLength(50)]
        public string CardNo { get; set; }

        [StringLength(50)]
        public string CardNumb { get; set; }

        public DateTime? EntranceTime { get; set; }

        [StringLength(250)]
        public string EntranceImage { get; set; }

        [StringLength(50)]
        public string EntranceGateID { get; set; }

        [StringLength(50)]
        public string EntranceOperatorID { get; set; }

        public DateTime? ExitTime { get; set; }

        [StringLength(250)]
        public string ExitImage { get; set; }

        [StringLength(50)]
        public string ExitGateID { get; set; }

        [StringLength(50)]
        public string ExitOperatorID { get; set; }

        [StringLength(50)]
        public string CarTypeID { get; set; }

        [StringLength(50)]
        public string CarModelID { get; set; }

        public bool? IsExit { get; set; }

        [StringLength(50)]
        public string AreaID { get; set; }

        [StringLength(50)]
        public string ParkingID { get; set; }

        public int? ReleaseType { get; set; }

        public int? EnterType { get; set; }

        public int? DataStatus { get; set; }

        public bool? MHOut { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? HaveUpdate { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        [StringLength(50)]
        public string EntranceCertificateNo { get; set; }

        [StringLength(50)]
        public string ExitCertificateNo { get; set; }

        [StringLength(250)]
        public string EntranceCertificateImage { get; set; }

        [StringLength(250)]
        public string ExitcertificateImage { get; set; }

        public int? PlateColor { get; set; }

        public int? IsOffline { get; set; }

        public int? OfflineID { get; set; }

        [StringLength(250)]
        public string EntranceIDCardPhoto { get; set; }

        [StringLength(250)]
        public string ExitIDCardPhoto { get; set; }

        public bool? IsScanCodeIn { get; set; }

        public bool? IsScanCodeOut { get; set; }

        [Column(TypeName = "image")]
        public byte[] ResFeature { get; set; }

        [StringLength(20)]
        public string LogName { get; set; }

        [StringLength(50)]
        public string EnterDistinguish { get; set; }

        [StringLength(50)]
        public string ExitDistinguish { get; set; }

        public int? IsUpdateIn { get; set; }

        public int? IsUpdateOut { get; set; }

        [StringLength(50)]
        public string ReserveBitID { get; set; }

        [StringLength(50)]
        public string EntranceCertName { get; set; }

        [StringLength(10)]
        public string EntranceSex { get; set; }

        [StringLength(50)]
        public string EntranceNation { get; set; }

        public DateTime? EntranceBirthDate { get; set; }

        [StringLength(250)]
        public string EntranceAddress { get; set; }

        [StringLength(50)]
        public string ExitCertName { get; set; }

        [StringLength(10)]
        public string ExitSex { get; set; }

        [StringLength(50)]
        public string ExitNation { get; set; }

        public DateTime? ExitBirthDate { get; set; }

        [StringLength(250)]
        public string ExitAddress { get; set; }

        public int? IsUpdateInOut { get; set; }

        [StringLength(50)]
        public string DHOnlienOrderID { get; set; }

        public int? BindPlate { get; set; }

        public decimal? NetWeight { get; set; }

        public decimal? Tare { get; set; }

        [StringLength(250)]
        public string Goods { get; set; }

        [StringLength(50)]
        public string Shipper { get; set; }

        [StringLength(50)]
        public string Shippingspace { get; set; }

        [StringLength(50)]
        public string DocumentsNo { get; set; }

        public int? IsUpdateXJNBOut { get; set; }

        public int? IsUpdateXJNBIn { get; set; }

        public int? IsUpdateTFGSDRZIn { get; set; }

        public int? IsUpdateTFGSDRZOut { get; set; }

        public int? JGJUpLoadIn { get; set; }

        public int? JGJUpLoadOut { get; set; }

        [StringLength(50)]
        public string VisitorID { get; set; }
    }
}
