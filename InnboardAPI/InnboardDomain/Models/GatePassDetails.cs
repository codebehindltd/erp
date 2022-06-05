namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GatePassDetails
    {
        [Key]
        public long GatePassItemId { get; set; }

        public long? GatePassId { get; set; }

        public int? CostCenterId { get; set; }

        public int? ItemId { get; set; }

        public int? StockById { get; set; }

        public decimal? Quantity { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public int? ReturnType { get; set; }

        public DateTime? ReturnDate { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public decimal? ApprovedQuantity { get; set; }
    }
}
