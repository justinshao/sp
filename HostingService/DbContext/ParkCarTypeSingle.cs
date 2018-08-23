namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ParkCarTypeSingle")]
    public partial class ParkCarTypeSingle
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string SingleID { get; set; }

        [StringLength(50)]
        public string CarTypeID { get; set; }

        public int? Week { get; set; }

        public int? SingleType { get; set; }

        public int? DataStatus { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? HaveUpdate { get; set; }
    }
}
