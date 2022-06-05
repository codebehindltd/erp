namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvItemStockInformation")]
    public partial class InvItemStockInformation
    {
        [Key]
        public int StockId { get; set; }

        public int? LocationId { get; set; }

        public int? ItemId { get; set; }

        public decimal? StockQuantity { get; set; }
    }
}
