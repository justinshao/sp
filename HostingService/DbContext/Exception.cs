namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Exception
    {
        public int ID { get; set; }

        public DateTime DateTime { get; set; }

        [StringLength(100)]
        public string Source { get; set; }

        [StringLength(20)]
        public string Server { get; set; }

        [StringLength(400)]
        public string Description { get; set; }

        [StringLength(2000)]
        public string Detail { get; set; }

        [StringLength(2000)]
        public string Track { get; set; }

        public int LogFrom { get; set; }
    }
}
