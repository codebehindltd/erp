namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SalesService")]
    public partial class SalesService
    {
        [Key]
        [Column(Order = 0)]
        public int ServiceId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CategoryId { get; set; }

        public decimal? PurchasePrice { get; set; }

        public int? SellingLocalCurrencyId { get; set; }

        public decimal? UnitPriceLocal { get; set; }

        public int? SellingUsdCurrencyId { get; set; }

        public decimal? UnitPriceUsd { get; set; }

        [StringLength(20)]
        public string Frequency { get; set; }

        public int? BandwidthType { get; set; }

        public int? Bandwidth { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
