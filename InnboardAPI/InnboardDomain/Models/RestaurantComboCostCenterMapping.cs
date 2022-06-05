namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantComboCostCenterMapping")]
    public partial class RestaurantComboCostCenterMapping
    {
        [Key]
        public int MappingId { get; set; }

        public int? CostCenterId { get; set; }

        public int? ComboId { get; set; }

        public decimal? MinimumStockLevel { get; set; }

        public decimal? StockQuantity { get; set; }

        public int? SellingLocalCurrencyId { get; set; }

        public decimal? UnitPriceLocal { get; set; }

        public int? SellingUsdCurrencyId { get; set; }

        public decimal? UnitPriceUsd { get; set; }
    }
}
