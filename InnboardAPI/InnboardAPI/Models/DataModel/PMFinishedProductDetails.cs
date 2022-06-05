namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PMFinishedProductDetails
    {
        [Key]
        public int FinishedProductDetailsId { get; set; }

        public int FinishProductId { get; set; }

        public int ProductId { get; set; }

        public int StockById { get; set; }

        public decimal Quantity { get; set; }
    }
}
