namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CommonDocuments
    {
        [Key]
        public long DocumentId { get; set; }

        public long? OwnerId { get; set; }

        [StringLength(100)]
        public string DocumentCategory { get; set; }

        [StringLength(100)]
        public string DocumentType { get; set; }

        [StringLength(50)]
        public string Extention { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Path { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedByDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
