namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WX_Info
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string OpenID { get; set; }

        [StringLength(50)]
        public string AccountID { get; set; }

        public int? UserType { get; set; }

        public int? FollowState { get; set; }

        [StringLength(50)]
        public string NickName { get; set; }

        [StringLength(20)]
        public string Language { get; set; }

        [StringLength(20)]
        public string Province { get; set; }

        [StringLength(20)]
        public string City { get; set; }

        [StringLength(20)]
        public string Country { get; set; }

        [StringLength(2)]
        public string Sex { get; set; }

        [StringLength(200)]
        public string Headimgurl { get; set; }

        public int? SubscribeTimes { get; set; }

        public DateTime? LastSubscribeDate { get; set; }

        public DateTime? LastUnsubscribeDate { get; set; }

        public DateTime? LastVisitDate { get; set; }

        [StringLength(50)]
        public string LastPlateNumber { get; set; }

        [StringLength(50)]
        public string CompanyID { get; set; }
    }
}
