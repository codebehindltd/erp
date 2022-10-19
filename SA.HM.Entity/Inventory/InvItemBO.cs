using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvItemBO
    {
        public int ItemId { get; set; }
        public int TypeId { get; set; }
        public string ItemType { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Code { get; set; }
        public string CodeAndName { get; set; }
        public string Model { get; set; }
        public string ItemNameAndCode { get; set; }
        public string Description { get; set; }
        public int ManufacturerId { get; set; }
        public int CategoryId { get; set; }
        public int CountryId { get; set; }
        public string CategoryName { get; set; }
        public string ProductType { get; set; }
        public int ClassificationId { get; set; }
        public decimal PurchasePrice { get; set; }
        public int SellingLocalCurrencyId { get; set; }
        public int SellingUsdCurrencyId { get; set; }
        public decimal UnitPriceLocal { get; set; }
        public decimal ItemUnit { get; set; }
        public decimal UnitPriceUsd { get; set; }
        public decimal MinimumStockLevel { get; set; }
        public decimal StockQuantity { get; set; }
        public string SerialNumber { get; set; }
        public decimal Quantity { get; set; }
        public decimal PurchaseQuantity { get; set; }
        public int ServiceWarranty { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public int RandomItemId { get; set; }
        public string StockType { get; set; }
        public int StockBy { get; set; }
        public string UnitHead { get; set; }
        public int SalesStockBy { get; set; }
        public string HeadName { get; set; }
        public string ImageName { get; set; }
        public Boolean IsCustomerItem { get; set; }
        public Boolean IsSupplierItem { get; set; }
        public int SupplierId { get; set; }
        public bool IsRecipe { get; set; }
        public int CostCenterId { get; set; }
        public int StockById { get; set; }
        public string AdjustmentFrequency { get; set; }
        public string IsLocalOrForeignPO { get; set; }
        public string Hierarchy { get; set; }
        public bool IsItemEditable { get; set; }
        public bool IsAttributeItem { get; set; }
        public decimal AverageCost { get; set; }
        public decimal Discount { get; set; }
        public decimal CitySDCharge { get; set; }
        public decimal AdditionalCharge { get; set; }
        public decimal VatAmount { get; set; }
        public decimal ServiceCharge { get; set; }
        public string AdditionalChargeType { get; set; }
        public string ServiceType { get; set; }
        public string ItemAttribute { get; set; }

        public string ItemWiseDiscountType { get; set; }
        public decimal ItemWiseIndividualDiscount { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
        public string StyleName { get; set; }
    }
}
