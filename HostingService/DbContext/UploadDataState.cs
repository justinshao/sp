namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UploadDataState")]
    public partial class UploadDataState
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string TableName { get; set; }

        [StringLength(50)]
        public string RecordID { get; set; }

        [StringLength(50)]
        public string ProxyNo { get; set; }

        public int? HaveUpdate { get; set; }

        public DateTime? LastUpdateTime { get; set; }
    }
}
