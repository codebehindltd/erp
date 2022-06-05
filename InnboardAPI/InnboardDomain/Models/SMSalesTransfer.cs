namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMSalesTransfer")]
    public partial class SMSalesTransfer
    {
        [Key]
        public long SalesTransferId { get; set; }

        public long? DealId { get; set; }

        public long QuotationId { get; set; }

        public int CostCenterId { get; set; }

        public int LocationId { get; set; }

        [StringLength(50)]
        public string DeliverStatus { get; set; }

        public DateTime Date { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public bool? IsDeleted { get; set; }

        public bool IsApproved { get; set; }

        [StringLength(100)]
        public string Remarks { get; set; }

        [StringLength(15)]
        public string TransferNumber { get; set; }

        public int? ApprovedBy { get; set; }
    }
}
