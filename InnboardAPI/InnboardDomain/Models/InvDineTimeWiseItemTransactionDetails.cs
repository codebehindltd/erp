namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InvDineTimeWiseItemTransactionDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TransactionDetailsId { get; set; }

        public long TransactionId { get; set; }

        public int CostCenterId { get; set; }

        public int LocationId { get; set; }

        public TimeSpan DineTime { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalSales { get; set; }

        [Column(TypeName = "money")]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal ServiceCharge { get; set; }

        [Column(TypeName = "money")]
        public decimal VatAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal GrnadTotal { get; set; }

        [Column(TypeName = "money")]
        public decimal NetSales { get; set; }

        [Column(TypeName = "money")]
        public decimal TTLNetSales { get; set; }

        public short Pax { get; set; }

        public decimal TTLPax { get; set; }

        public decimal AverageGuest { get; set; }

        public decimal Checks { get; set; }

        public decimal TTLChecks { get; set; }

        public decimal AverageChecks { get; set; }
    }
}
