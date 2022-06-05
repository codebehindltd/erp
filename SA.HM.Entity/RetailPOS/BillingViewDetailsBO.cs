using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.RetailPOS
{
    public class BillingViewDetailsBO
    {
        public int ItemId { get; set; }
        public string ItemType { get; set; }
        public decimal Quantity { get; set; }
        public long KotDetailId { get; set; }
        public Nullable<decimal> RemainingDeliveryQuantity { get; set; }
        public decimal? DeliveredQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockBy { get; set; }
        public string ItemName { get; set; }
        public string ProductType { get; set; }
        public string HeadName { get; set; }
        public Nullable<decimal> StockQuantity { get; set; }
        public long SalesTransferDetailId { get; set; }
    }
}
