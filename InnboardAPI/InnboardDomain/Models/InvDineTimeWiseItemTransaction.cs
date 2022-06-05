namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvDineTimeWiseItemTransaction")]
    public partial class InvDineTimeWiseItemTransaction
    {
        [Key]
        public long TransactionId { get; set; }

        [Column(TypeName = "date")]
        public DateTime TransactionDate { get; set; }

        public decimal TotalSalesQuantity { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalSales { get; set; }

        [Column(TypeName = "money")]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal Netsales { get; set; }

        [Column(TypeName = "money")]
        public decimal ServiceCharge { get; set; }

        [Column(TypeName = "money")]
        public decimal VatAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal GrandTotal { get; set; }

        public int TotalPax { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalRevenue { get; set; }

        public int TotalVoid { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalVoidAmount { get; set; }

        public short ErrorCorrects { get; set; }

        [Column(TypeName = "money")]
        public decimal ErrorCorrectsAmount { get; set; }

        public short Checks { get; set; }

        [Column(TypeName = "money")]
        public decimal ChecksAmount { get; set; }

        public short ChecksPaid { get; set; }

        [Column(TypeName = "money")]
        public decimal ChecksPaidAmount { get; set; }

        public short Outstanding { get; set; }

        [Column(TypeName = "money")]
        public decimal OutstandingAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalCashPayment { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalCardPayment { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalRefund { get; set; }
    }
}
