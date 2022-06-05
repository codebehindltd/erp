using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class PMProductReceivedReportBO : PMProductReceivedBO
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int StockById { get; set; }
        public string HeadName { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string SupplierName { get; set; }
        public string SerialNumber { get; set; }

    }
}
