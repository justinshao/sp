namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PersonTg")]
    public partial class PersonTg
    {
        public int id { get; set; }

        [StringLength(30)]
        public string name { get; set; }

        [StringLength(30)]
        public string phone { get; set; }

        [StringLength(200)]
        public string bz { get; set; }

        public int? count { get; set; }
    }
}
