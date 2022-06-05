namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMQuotation")]
    public partial class SMQuotation
    {
        [Key]
        public long QuotationId { get; set; }

        [Required]
        [StringLength(25)]
        public string QuotationNo { get; set; }

        public int CompanyId { get; set; }

        [Column(TypeName = "date")]
        public DateTime ProposalDate { get; set; }

        public int ServiceTypeId { get; set; }

        public int LocationId { get; set; }

        public int SiteId { get; set; }

        public short TotalDeviceOrUser { get; set; }

        public int ContractPeriodId { get; set; }

        public int BillingPeriodId { get; set; }

        public int ItemServiceDeliveryId { get; set; }

        public int? CurrentVendorId { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
