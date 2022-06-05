using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class QuotationViewDetailsBO
    {
        public int ItemId { get; set; }
        public string ItemType { get; set; }
        public decimal Quantity { get; set; }
        public long QuotationDetailsId { get; set; }
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
