namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PMSalesDetail")]
    public partial class PMSalesDetail
    {
        [Key]
        public int DetailId { get; set; }

        public int? SalesId { get; set; }

        [StringLength(50)]
        public string ServiceType { get; set; }

        public int? ItemId { get; set; }

        public decimal? ItemUnit { get; set; }

        public int? SellingLocalCurrencyId { get; set; }

        public decimal? UnitPriceLocal { get; set; }

        public int? SellingUsdCurrencyId { get; set; }

        public decimal? UnitPriceUsd { get; set; }
    }
}
