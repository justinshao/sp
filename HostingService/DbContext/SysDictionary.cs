namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SysDictionary")]
    public partial class SysDictionary
    {
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string CatagrayName { get; set; }

        [Required]
        [StringLength(50)]
        public string DicValue { get; set; }

        public int OrderID { get; set; }

        [StringLength(50)]
        public string Des { get; set; }

        public int DicID { get; set; }
    }
}
