namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OperateLog")]
    public partial class OperateLog
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Operator { get; set; }

        [StringLength(20)]
        public string OperatorIP { get; set; }

        public DateTime OperateTime { get; set; }

        [Required]
        [StringLength(100)]
        public string ModuleName { get; set; }

        [StringLength(100)]
        public string MethodName { get; set; }

        public int LogFrom { get; set; }

        public int OperatorType { get; set; }

        [Required]
        [StringLength(4000)]
        public string OperatorContent { get; set; }
    }
}
