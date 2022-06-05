namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InvItemTransactionPaymentDetails
    {
        [Key]
        public long TransactionPaymentId { get; set; }

        public long TransactionId { get; set; }

        [Column(TypeName = "date")]
        public DateTime TransactionDate { get; set; }

        public int? LocationId { get; set; }

        [StringLength(25)]
        public string PaymentType { get; set; }

        [StringLength(350)]
        public string CardType { get; set; }

        [Column(TypeName = "money")]
        public decimal? PaymentAmount { get; set; }
    }
}
