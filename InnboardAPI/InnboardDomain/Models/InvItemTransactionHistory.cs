namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvItemTransactionHistory")]
    public partial class InvItemTransactionHistory
    {
        [Key]
        public long ItemTransactionId { get; set; }

        public DateTime TransactionDate { get; set; }

        [Required]
        public string TransactionType { get; set; }

        public long? TransactionForId { get; set; }

        public string TransactionForType { get; set; }

        public string TransactionFor { get; set; }

        public int? CostCenterId { get; set; }

        public int? LocationId { get; set; }

        public int? ToCostCenterId { get; set; }

        public int? ToLocationId { get; set; }

        public string CostCenter { get; set; }

        public string Location { get; set; }

        public string ToCostCenter { get; set; }

        public string ToLocation { get; set; }

        public int ItemId { get; set; }

        [Required]
        [StringLength(500)]
        public string ItemName { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? AverageCost { get; set; }

        public decimal? TransactionTotalCost { get; set; }

        public string RefDocNo { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
