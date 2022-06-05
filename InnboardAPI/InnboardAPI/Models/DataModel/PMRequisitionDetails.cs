namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PMRequisitionDetails
    {
        [Key]
        public long RequisitionDetailsId { get; set; }

        public int RequisitionId { get; set; }

        public int CategoryId { get; set; }

        public int ItemId { get; set; }

        public int StockById { get; set; }

        public decimal Quantity { get; set; }

        public decimal? ApprovedQuantity { get; set; }

        public decimal? DeliveredQuantity { get; set; }

        public decimal DelivarOutQuantity { get; set; }

        [StringLength(20)]
        public string ApprovedStatus { get; set; }

        [StringLength(150)]
        public string Remarks { get; set; }
    }
}
