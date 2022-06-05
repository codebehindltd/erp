namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantSalesReturnItem")]
    public partial class RestaurantSalesReturnItem
    {
        [Key]
        public long ReturnId { get; set; }

        public int BillId { get; set; }

        public int KotId { get; set; }

        public int CostCenterId { get; set; }

        public int ItemId { get; set; }

        [Required]
        [StringLength(300)]
        public string ItemName { get; set; }

        public decimal ItemUnit { get; set; }

        public decimal UnitRate { get; set; }

        public decimal Amount { get; set; }

        public decimal AverageCost { get; set; }

        public decimal ReturnedUnit { get; set; }

        public decimal LastReturnedUnit { get; set; }

        public DateTime? LastReturnDate { get; set; }

        public decimal? ItemTotalAmount { get; set; }

        public decimal? DiscountAmount { get; set; }

        public decimal? DiscountedAmount { get; set; }

        public decimal? ServiceRate { get; set; }

        public decimal? ServiceCharge { get; set; }

        public decimal? VatAmount { get; set; }

        public decimal? CitySDCharge { get; set; }

        public decimal? AdditionalCharge { get; set; }

        public decimal? ItemCost { get; set; }

        public decimal? InvoiceDiscount { get; set; }
    }
}
