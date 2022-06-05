namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvItemTransaction")]
    public partial class InvItemTransaction
    {
        [Key]
        public long TransactionId { get; set; }

        public DateTime TransactionDate { get; set; }

        [StringLength(150)]
        public string StartBillNumber { get; set; }

        [StringLength(150)]
        public string EndingBillNumber { get; set; }

        public short? TotalBillCount { get; set; }

        public decimal? TotalSalesQuantity { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalSalesAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalDiscountAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalNetSalesAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalServiceChargeAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalVatAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? GrossSalesAmount { get; set; }

        public short? TotalVoidQuantity { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalVoidAmount { get; set; }

        public short? TotalError { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalErrorAmount { get; set; }

        public short? TotalPax { get; set; }

        public decimal? TotalVariance { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalVarianceAmount { get; set; }

        public int? WastageEntryById { get; set; }

        [StringLength(450)]
        public string WastageEntryByName { get; set; }

        public decimal? TotalStockCountDeifference { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalStockCountDeifferenceAmount { get; set; }

        public int? AdjustmentEntryById { get; set; }

        [StringLength(450)]
        public string AdjustmentEntryByName { get; set; }

        public decimal? TotalReceivedQuantity { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalReceivedAmount { get; set; }

        public decimal? TotalUsageQuantity { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalUsageCost { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalCashPayment { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalCardPayment { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalPayment { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalRefundAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalRevenue { get; set; }

        public decimal? TotalPerGuestUsageQuantity { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalPerGuestUsageAmount { get; set; }
    }
}
