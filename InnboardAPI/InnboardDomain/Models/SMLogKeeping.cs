namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMLogKeeping")]
    public partial class SMLogKeeping
    {
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime LogDateTime { get; set; }

        public int? CompanyId { get; set; }

        public long? ContactId { get; set; }

        public long? DealId { get; set; }

        public long? SalesCallEntryId { get; set; }

        public int CreatedBy { get; set; }
    }
}
