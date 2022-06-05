namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InvDineTimeWisePaymentDetails
    {
        [Key]
        public long TransactionPaymentId { get; set; }

        public long TransactionId { get; set; }

        [Column(TypeName = "date")]
        public DateTime TransactionDate { get; set; }

        public TimeSpan DineTimeFrom { get; set; }

        public TimeSpan DineTimeTo { get; set; }

        public int CostCenterId { get; set; }

        public int LocationId { get; set; }

        [Required]
        [StringLength(25)]
        public string PaymentType { get; set; }

        [StringLength(250)]
        public string CardType { get; set; }

        [Column(TypeName = "money")]
        public decimal PaymentAmount { get; set; }

        public int PaymentNo { get; set; }
    }
}
