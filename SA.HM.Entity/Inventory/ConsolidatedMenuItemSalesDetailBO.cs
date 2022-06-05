using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class ConsolidatedMenuItemSalesDetailBO
    {
        public int? BillId { get; set; }
        public DateTime? BillDate { get; set; }
        public int? KotId { get; set; }
        public int? ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string MLVL { get; set; }
        public decimal? SalesPrice { get; set; }
        public decimal? SalesQuantity { get; set; }
        public decimal? SalesTTL { get; set; }
        public decimal? ReturnQuantity { get; set; }
        public decimal? ReturnPrice { get; set; }
        public decimal? ReturnTTL { get; set; }
        public decimal? GrossSales { get; set; }
        public decimal? GrossSalesTTL { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountTTL { get; set; }
        public decimal? NetSales { get; set; }
        public decimal? NetSaleTTL { get; set; }
        public decimal? ServiceCharge { get; set; }
        public decimal? VatAmount { get; set; }
        public int? PaxQuantity { get; set; }
    }
}
