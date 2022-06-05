namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SMSalesTransferDetails
    {
        [Key]
        public long SalesTransferDetailId { get; set; }

        public long SalesTransferId { get; set; }

        public int ItemId { get; set; }

        public decimal? AverageCost { get; set; }

        public decimal Quantity { get; set; }

        public int? StockById { get; set; }
    }
}
