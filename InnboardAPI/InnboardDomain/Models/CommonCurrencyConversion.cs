namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonCurrencyConversion")]
    public partial class CommonCurrencyConversion
    {
        [Key]
        public int ConversionId { get; set; }

        public int? FromCurrencyId { get; set; }

        public int? ToCurrencyId { get; set; }

        public decimal? ConversionRate { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedByDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedByDate { get; set; }
    }
}
