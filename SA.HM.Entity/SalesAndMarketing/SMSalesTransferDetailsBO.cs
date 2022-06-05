using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMSalesTransferDetailsBO
    {
        public long SalesTransferDetailId { get; set; }
        public long SalesTransferId { get; set; }
        public int ItemId { get; set; }
        public Nullable<int> ManufacturerId { get; set; }
        public Nullable<decimal> AverageCost { get; set; }
        public decimal Quantity { get; set; }
        public Nullable<decimal> QuotationQuantity { get; set; }
        public Nullable<int> StockById { get; set; }

        public string ItemName { get; set; }
        public string StockBy { get; set; }
    }
}
