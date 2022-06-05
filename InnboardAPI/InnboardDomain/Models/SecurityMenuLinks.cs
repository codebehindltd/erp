namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SecurityMenuLinks
    {
        [Key]
        public long MenuLinksId { get; set; }

        public int ModuleId { get; set; }

        [Required]
        [StringLength(250)]
        public string PageId { get; set; }

        [StringLength(50)]
        public string PageName { get; set; }

        [StringLength(50)]
        public string PageDisplayCaption { get; set; }

        [Required]
        [StringLength(25)]
        public string PageExtension { get; set; }

        [Required]
        [StringLength(200)]
        public string PagePath { get; set; }

        [StringLength(6)]
        public string PageType { get; set; }

        [StringLength(25)]
        public string LinkIconClass { get; set; }

        public bool ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
