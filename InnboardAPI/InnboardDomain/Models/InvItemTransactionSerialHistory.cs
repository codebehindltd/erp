namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvItemTransactionSerialHistory")]
    public partial class InvItemTransactionSerialHistory
    {
        [Key]
        public long ItemSerialTransactionId { get; set; }

        public long ItemTransactionId { get; set; }

        public DateTime TransactionDate { get; set; }

        public int? CostCenterId { get; set; }

        public int? LocationId { get; set; }

        public int ItemId { get; set; }

        [Required]
        [StringLength(50)]
        public string SerialNumber { get; set; }
    }
}
