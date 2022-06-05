namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PMSupplierProductReturnDetails
    {
        [Key]
        public long ReturnDetailsId { get; set; }

        public long ReturnId { get; set; }

        public int StockById { get; set; }

        public int ItemId { get; set; }

        public decimal Quantity { get; set; }

        public decimal? AverageCost { get; set; }

        public decimal OrderQuantity { get; set; }
    }
}
