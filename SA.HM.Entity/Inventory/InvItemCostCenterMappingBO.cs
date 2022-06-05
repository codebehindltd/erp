using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvItemCostCenterMappingBO
    {
        public int MappingId { get; set; }
        public int CostCenterId { get; set; }
        public int ItemId { get; set; }
        public int KitchenId { get; set; }
        public decimal MinimumStockLevel { get; set; }
        public decimal UnitPriceLocal { get; set; }
        public decimal UnitPriceUsd { get; set; }
        public decimal StockQuantity { get; set; }
        public DateTime AdjustmentLastDate { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal SDCharge { get; set; }
        public decimal VatAmount { get; set; }
        public decimal AdditionalCharge { get; set; }
    }
}
