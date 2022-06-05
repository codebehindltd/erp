namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InvItemStockAdjustmentDetails
    {
        [Key]
        public int StockAdjustmentDetailsId { get; set; }

        public int StockAdjustmentId { get; set; }

        public int ItemId { get; set; }

        public int LocationId { get; set; }

        public int StockById { get; set; }

        public decimal? OpeningQuantity { get; set; }

        public decimal? ReceiveQuantity { get; set; }

        public decimal? ActualUsage { get; set; }

        public decimal? WastageQuantity { get; set; }

        public decimal? StockQuantity { get; set; }

        public decimal? ActualQuantity { get; set; }

        public decimal? VarianceQuantity { get; set; }

        public decimal? AverageCost { get; set; }
    }
}
