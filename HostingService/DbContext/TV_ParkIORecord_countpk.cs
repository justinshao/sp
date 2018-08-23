namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TV_ParkIORecord_countpk
    {
        [Key]
        [StringLength(25)]
        public string PKName { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [StringLength(50)]
        public string pkid { get; set; }

        public int? cs { get; set; }

        public DateTime? EntranceTime { get; set; }

        public DateTime? ExitTime { get; set; }
    }
}
