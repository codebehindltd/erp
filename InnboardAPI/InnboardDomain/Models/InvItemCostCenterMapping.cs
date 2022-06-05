namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvItemCostCenterMapping")]
    public partial class InvItemCostCenterMapping
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int MappingId { get; set; }

        public int? CostCenterId { get; set; }

        public int? ItemId { get; set; }

        public int? KitchenId { get; set; }

        public decimal? MinimumStockLevel { get; set; }

        public int? SellingLocalCurrencyId { get; set; }

        public decimal? UnitPriceLocal { get; set; }

        public int? SellingUsdCurrencyId { get; set; }

        public decimal? UnitPriceUsd { get; set; }

        public decimal? ServiceCharge { get; set; }

        public decimal? SDCharge { get; set; }

        public decimal? VatAmount { get; set; }

        public decimal? AdditionalCharge { get; set; }

        [StringLength(50)]
        public string DiscountType { get; set; }

        public decimal? DiscountAmount { get; set; }

        public decimal? StockQuantity { get; set; }

        public DateTime? AdjustmentLastDate { get; set; }

        public decimal? AverageCostDelete { get; set; }

        public int? CategoryId { get; set; }
    }
}
