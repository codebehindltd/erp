namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InvItemTransactionDetails
    {
        [Key]
        public long TransactionDetailsId { get; set; }

        public long TransactionId { get; set; }

        public int? LocationId { get; set; }

        public int? CategoryId { get; set; }

        [StringLength(50)]
        public string ItemCode { get; set; }

        public int? ItemId { get; set; }

        public string ItemName { get; set; }

        public bool? IsCustomerItem { get; set; }

        public int? StockById { get; set; }

        [StringLength(150)]
        public string StockBy { get; set; }

        public decimal? AverageCost { get; set; }

        public decimal? BeginingStockQuantity { get; set; }

        public decimal? PurchaseQuantity { get; set; }

        [Column(TypeName = "money")]
        public decimal? PurchasePrice { get; set; }

        public decimal? UsageQuantity { get; set; }

        public decimal? AverageUsageCost { get; set; }

        public decimal? WastageQuantity { get; set; }

        [Column(TypeName = "money")]
        public decimal? WastageCost { get; set; }

        [StringLength(300)]
        public string WastageAllowance { get; set; }

        [StringLength(300)]
        public string WastageReason { get; set; }

        public decimal? AdjustmentStockQuantity { get; set; }

        public decimal? DayEndStockQuantity { get; set; }

        public decimal? StockCountDifference { get; set; }

        [Column(TypeName = "money")]
        public decimal? StockCountDifferenceAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? PriceToday { get; set; }

        [Column(TypeName = "money")]
        public decimal? PriceYestarday { get; set; }

        [Column(TypeName = "money")]
        public decimal? PriceFluctuation { get; set; }

        public decimal? SalesQuantity { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal? Amount { get; set; }

        public decimal? DiscountedAmount { get; set; }

        public decimal? ServiceRate { get; set; }

        public decimal? Discount { get; set; }

        [Column(TypeName = "money")]
        public decimal? Vat { get; set; }

        [Column(TypeName = "money")]
        public decimal? ServiceCharge { get; set; }

        public decimal? PerGuestUsageQuantity { get; set; }

        [Column(TypeName = "money")]
        public decimal? PerGuestUsageAmount { get; set; }
    }
}
