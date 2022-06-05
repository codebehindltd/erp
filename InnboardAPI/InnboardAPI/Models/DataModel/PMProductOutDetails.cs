namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PMProductOutDetails
    {
        [Key]
        public int OutDetailsId { get; set; }

        public int OutId { get; set; }

        public int? StockById { get; set; }

        public int ItemId { get; set; }

        public decimal Quantity { get; set; }

        public decimal? AverageCost { get; set; }

        public decimal? AdjustmentQuantity { get; set; }

        public int? AdjustmentStockById { get; set; }

        [StringLength(15)]
        public string ApprovalStatus { get; set; }

        public decimal? ApprovedQuantity { get; set; }
    }
}
