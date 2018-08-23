namespace HostingService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UploadImg
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string RecordID { get; set; }

        [StringLength(1000)]
        public string UploadFailedFile { get; set; }

        public DateTime? LastDateTime { get; set; }

        public int? ImgType { get; set; }

        public bool? IsClient { get; set; }
    }
}
