namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BaseCompany")]
    public partial class BaseCompany
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string CPID { get; set; }

        [Required]
        [StringLength(50)]
        public string CPName { get; set; }

        public int? CityID { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [StringLength(50)]
        public string LinkMan { get; set; }

        [StringLength(20)]
        public string Mobile { get; set; }

        [StringLength(50)]
        public string MasterID { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int HaveUpdate { get; set; }

        public int DataStatus { get; set; }
    }
}
