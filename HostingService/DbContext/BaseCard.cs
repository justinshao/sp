namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BaseCard")]
    public partial class BaseCard
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string CardID { get; set; }

        [StringLength(50)]
        public string EmployeeID { get; set; }

        [Required]
        [StringLength(50)]
        public string CardNo { get; set; }

        [StringLength(50)]
        public string CardNumb { get; set; }

        public int CardType { get; set; }

        public decimal Balance { get; set; }

        public int State { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int HaveUpdate { get; set; }

        public int DataStatus { get; set; }

        public decimal Deposit { get; set; }

        public DateTime RegisterTime { get; set; }

        [Required]
        [StringLength(50)]
        public string OperatorID { get; set; }

        public int CardSystem { get; set; }

        [Required]
        [StringLength(50)]
        public string VID { get; set; }
    }
}
