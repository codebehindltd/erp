namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelCurrencyConversion")]
    public partial class HotelCurrencyConversion
    {
        [Key]
        public int ConversionId { get; set; }

        public int? ConversionRateId { get; set; }

        public decimal? ConversionAmount { get; set; }

        public decimal? ConversionRate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedByDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedByDate { get; set; }
    }
}
