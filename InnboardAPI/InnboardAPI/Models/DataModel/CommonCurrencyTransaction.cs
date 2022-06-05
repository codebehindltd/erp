namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonCurrencyTransaction")]
    public partial class CommonCurrencyTransaction
    {
        [Key]
        public int CurrencyConversionId { get; set; }

        [StringLength(20)]
        public string TransactionNumber { get; set; }

        public int? FromConversionHeadId { get; set; }

        public int? ToConversionHeadId { get; set; }

        public decimal? ConversionAmount { get; set; }

        public decimal? ConversionRate { get; set; }

        public decimal? ConvertedAmount { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
