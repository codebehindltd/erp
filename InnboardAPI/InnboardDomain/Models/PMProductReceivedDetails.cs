namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PMProductReceivedDetails
    {
        [Key]
        public int ReceiveDetailsId { get; set; }

        public int ReceivedId { get; set; }

        public int ItemId { get; set; }

        public int StockById { get; set; }

        public decimal Quantity { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal? AverageCost { get; set; }

        public decimal? ApprovedReturnQuantity { get; set; }

        public decimal? ReturnQuantity { get; set; }
    }
}
