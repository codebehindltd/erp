namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PMSupplierProductReturnDetails
    {
        [Key]
        public int ReturnDetailsId { get; set; }

        public int ReturnId { get; set; }

        public int? CostCenterIdFrom { get; set; }

        public int? LocationIdFrom { get; set; }

        public int? CostCenterIdTo { get; set; }

        public int? LocationIdTo { get; set; }

        public int? StockById { get; set; }

        public int ProductId { get; set; }

        public decimal Quantity { get; set; }

        public decimal? AverageCost { get; set; }
    }
}
