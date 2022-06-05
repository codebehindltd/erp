namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SMQuotationDetails
    {
        [Key]
        public long QuotationDetailsId { get; set; }

        public long QuotationId { get; set; }

        [Required]
        [StringLength(25)]
        public string ItemType { get; set; }

        public int CategoryId { get; set; }

        public int? ServicePackageId { get; set; }

        public int? ServiceBandWidthId { get; set; }

        public int? ServiceTypeId { get; set; }

        public int ItemId { get; set; }

        public int StockBy { get; set; }

        public decimal Quantity { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalPrice { get; set; }

        public int? UpLink { get; set; }

        [StringLength(250)]
        public string SalesNote { get; set; }

        public decimal? RemainingDeliveryQuantity { get; set; }
    }
}
