namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WX_UserLocation
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string OpenId { get; set; }

        [Required]
        [StringLength(30)]
        public string Latitude { get; set; }

        [Required]
        [StringLength(30)]
        public string Longitude { get; set; }

        [Required]
        [StringLength(30)]
        public string Precision { get; set; }

        public DateTime LastReportedTime { get; set; }

        [StringLength(50)]
        public string CompanyID { get; set; }
    }
}
