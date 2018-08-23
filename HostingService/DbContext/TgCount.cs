namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TgCount")]
    public partial class TgCount
    {
        public int id { get; set; }

        [StringLength(300)]
        public string keyid { get; set; }

        [StringLength(50)]
        public string time { get; set; }

        [StringLength(200)]
        public string username { get; set; }

        [Required]
        [StringLength(200)]
        public string fromusername { get; set; }

        [StringLength(50)]
        public string times { get; set; }
    }
}
