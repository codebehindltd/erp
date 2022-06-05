namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PMPurchaseOrderDetails
    {
        [Key]
        public int DetailId { get; set; }

        public int? POrderId { get; set; }

        public int? RequisitionId { get; set; }

        public int? StockById { get; set; }

        public int? ItemId { get; set; }

        public decimal? PurchasePrice { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? QuantityReceived { get; set; }

        [StringLength(20)]
        public string MessureUnit { get; set; }

        public decimal? AverageCost { get; set; }

        public decimal? ActualReceivedQuantity { get; set; }

        [StringLength(250)]
        public string Remarks { get; set; }

        public decimal? RemainingReceiveQuantity { get; set; }
    }
}
