namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvItem")]
    public partial class InvItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int ItemId { get; set; }

        [StringLength(100)]
        public string ItemType { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public int? CategoryId { get; set; }

        [StringLength(50)]
        public string StockType { get; set; }

        public int? StockBy { get; set; }

        public int? SalesStockBy { get; set; }

        public int? ClassificationId { get; set; }

        public bool? IsCustomerItem { get; set; }

        public bool? IsSupplierItem { get; set; }

        public int? ManufacturerId { get; set; }

        [StringLength(20)]
        public string ProductType { get; set; }

        public decimal? PurchasePrice { get; set; }

        public int? SellingLocalCurrencyId { get; set; }

        public decimal? UnitPriceLocal { get; set; }

        public int? SellingUsdCurrencyId { get; set; }

        public decimal? UnitPriceUsd { get; set; }

        public int? ServiceWarranty { get; set; }

        [StringLength(250)]
        public string ImageName { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public decimal? AverageCost { get; set; }

        public bool? IsRecipe { get; set; }

        public int? SupplierId { get; set; }

        [StringLength(50)]
        public string AdjustmentFrequency { get; set; }

        public DateTime? AdjustmentLastDate { get; set; }

        public bool? IsItemEditable { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LastPurchaseDate { get; set; }
    }
}
