using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class DatewisePurchaseCompareViewBO
    {
        public DateTime? ReceivedDate { get; set; }
        public string ItemName { get; set; }
        public string StockBy { get; set; }
        public decimal? Unit { get; set; }
        public decimal? PurchaseRate { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? Variance { get; set; }
    }
}
