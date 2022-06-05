namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PMSalesInvoice")]
    public partial class PMSalesInvoice
    {
        [Key]
        public long InvoiceId { get; set; }

        [StringLength(20)]
        public string InvoiceNumber { get; set; }

        public DateTime? BillFromDate { get; set; }

        public DateTime? BillToDate { get; set; }

        public long? SalesId { get; set; }

        public decimal? InvoiceAmount { get; set; }

        public decimal? DueOrAdvanceAmount { get; set; }

        public DateTime? BillDueDate { get; set; }

        [StringLength(200)]
        public string InvoiceDetailId { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
