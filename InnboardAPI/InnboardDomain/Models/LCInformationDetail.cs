namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LCInformationDetail")]
    public partial class LCInformationDetail
    {
        [Key]
        public long LCDetailId { get; set; }

        public long LCId { get; set; }

        public int? POrderId { get; set; }

        public int? CostCenterId { get; set; }

        public int? StockById { get; set; }

        public int? ProductId { get; set; }

        public decimal? PurchasePrice { get; set; }

        public decimal? Quantity { get; set; }
    }
}
