namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PMSales
    {
        [Key]
        public int SalesId { get; set; }

        [StringLength(20)]
        public string BillNumber { get; set; }

        public DateTime? SalesDate { get; set; }

        public int? CustomerId { get; set; }

        public int? SiteInfoId { get; set; }

        public int? BillingInfoId { get; set; }

        public int? TechnicalInfoId { get; set; }

        public decimal? SalesAmount { get; set; }

        public decimal? VatAmount { get; set; }

        public decimal? GrandTotal { get; set; }

        public int? FieldId { get; set; }

        [StringLength(20)]
        public string Frequency { get; set; }

        public decimal? DueAmount { get; set; }

        public DateTime? BillExpireDate { get; set; }

        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }
    }
}
