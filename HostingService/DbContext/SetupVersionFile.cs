namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SetupVersionFile
    {
        public int ID { get; set; }

        [Required]
        [StringLength(36)]
        public string SetupVersionFilesID { get; set; }

        [Required]
        [StringLength(36)]
        public string SetupVersionID { get; set; }

        [Required]
        [StringLength(50)]
        public string FName { get; set; }

        [Required]
        [StringLength(10)]
        public string FType { get; set; }

        [Required]
        [StringLength(15)]
        public string FSize { get; set; }

        public short IsDownLoad { get; set; }

        [StringLength(100)]
        public string Remark { get; set; }
    }
}
