namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InvItemStockVarianceDetails
    {
        [Key]
        public int StockVarianceDetailsId { get; set; }

        public int StockVarianceId { get; set; }

        public int ItemId { get; set; }

        public int LocationId { get; set; }

        public int StockById { get; set; }

        public int TModeId { get; set; }

        public decimal? UsageQuantity { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal? UsageCost { get; set; }

        public decimal VarianceQuantity { get; set; }

        public decimal VarianceCost { get; set; }

        [Required]
        [StringLength(50)]
        public string Reason { get; set; }
    }
}
