namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvItemTransactionLogSummary")]
    public partial class InvItemTransactionLogSummary
    {
        [Key]
        public long ItemTransactionId { get; set; }

        public DateTime TransactionDate { get; set; }

        public int? LocationId { get; set; }

        public int ItemId { get; set; }

        public decimal? AverageCost { get; set; }

        public decimal DayOpeningQuantity { get; set; }

        public decimal TransactionalOpeningQuantity { get; set; }

        public decimal? ReceiveQuantity { get; set; }

        public decimal? OutItemQuantity { get; set; }

        public decimal? WastageQuantity { get; set; }

        public decimal? AdjustmentQuantity { get; set; }

        public decimal? SalesQuantity { get; set; }

        public decimal ClosingQuantity { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
