namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvItemTransactionLog")]
    public partial class InvItemTransactionLog
    {
        [Key]
        public long ItemTransactionId { get; set; }

        public DateTime TransactionDate { get; set; }

        [Required]
        [StringLength(25)]
        public string TransactionType { get; set; }

        public long? TransactionForId { get; set; }

        public int? LocationId { get; set; }

        public int ItemId { get; set; }

        public decimal? AverageCost { get; set; }

        public decimal DayOpeningQuantity { get; set; }

        public decimal TransactionalOpeningQuantity { get; set; }

        public decimal TransactionQuantity { get; set; }

        public decimal ClosingQuantity { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
