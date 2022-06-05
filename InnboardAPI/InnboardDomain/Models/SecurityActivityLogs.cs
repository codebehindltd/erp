namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SecurityActivityLogs
    {
        [Key]
        public long ActivityId { get; set; }

        [Required]
        [StringLength(256)]
        public string ActivityType { get; set; }

        [Required]
        [StringLength(256)]
        public string EntityType { get; set; }

        public long? EntityId { get; set; }

        [Required]
        [StringLength(256)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedByDate { get; set; }

        [StringLength(100)]
        public string Module { get; set; }
    }
}
