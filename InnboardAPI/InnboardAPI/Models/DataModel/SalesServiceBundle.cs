namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SalesServiceBundle")]
    public partial class SalesServiceBundle
    {
        [Key]
        public int BundleId { get; set; }

        [StringLength(100)]
        public string BundleName { get; set; }

        [StringLength(20)]
        public string BundleCode { get; set; }

        [StringLength(20)]
        public string Frequency { get; set; }

        public int? SellingPriceLocal { get; set; }

        public decimal? UnitPriceLocal { get; set; }

        public int? SellingPriceUsd { get; set; }

        public decimal? UnitPriceUsd { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
