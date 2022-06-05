namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantBuffetCostCenterMapping")]
    public partial class RestaurantBuffetCostCenterMapping
    {
        [Key]
        public int MappingId { get; set; }

        public int? CostCenterId { get; set; }

        public int? BuffetId { get; set; }

        public decimal? MinimumStockLevel { get; set; }

        public decimal? StockQuantity { get; set; }

        public int? SellingLocalCurrencyId { get; set; }

        public decimal? UnitPriceLocal { get; set; }

        public int? SellingUsdCurrencyId { get; set; }

        public decimal? UnitPriceUsd { get; set; }
    }
}
