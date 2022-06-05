using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvItemAutoSearchBO
    {
        public int KotDetailId { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string ItemName { get; set; }
        public string Code { get; set; }
        public string ItemNameAndCode { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ColorId { get; set; }
        public string ColorText { get; set; }
        public int SizeId { get; set; }
        public string SizeText { get; set; }
        public int StyleId { get; set; }
        public string StyleText { get; set; }
        public string ImageName { get; set; }
        public string ProductType { get; set; }
        public string StockType { get; set; }
        public int StockBy { get; set; }
        public string UnitHead { get; set; }
        public bool IsRecipe { get; set; }
        public decimal StockQuantity { get; set; }
        public decimal Quantity { get; set; }
        public decimal AverageCost { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
        public decimal UnitPriceLocal { get; set; }
        public decimal UnitPriceUsd { get; set; }        
        public decimal InvoiceDiscount { get; set; }
        public string ManufacturerName { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public decimal? ServiceCharge { get; set; }
        public decimal? SDCharge { get; set; }
        public decimal? VatAmount { get; set; }
        public decimal? AdditionalCharge { get; set; }
        public bool IsAttributeItem { get; set; }
    }
}
