namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMCompanySite")]
    public partial class SMCompanySite
    {
        [Key]
        public int SiteId { get; set; }

        public int CompanyId { get; set; }

        [Required]
        [StringLength(250)]
        public string SiteName { get; set; }

        [StringLength(250)]
        public string BusinessContactName { get; set; }

        [StringLength(150)]
        public string BusinessContactEmail { get; set; }

        [StringLength(25)]
        public string BusinessContactPhone { get; set; }

        [StringLength(250)]
        public string TechnicalContactName { get; set; }

        [StringLength(150)]
        public string TechnicalContactEmail { get; set; }

        [StringLength(25)]
        public string TechnicalContactPhone { get; set; }

        [StringLength(250)]
        public string BillingContactName { get; set; }

        [StringLength(150)]
        public string BillingContactEmail { get; set; }

        [StringLength(25)]
        public string BillingContactPhone { get; set; }

        [StringLength(350)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
