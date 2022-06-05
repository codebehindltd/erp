using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class ItemSalesPopularityInfoBO
    {
        public string CostCenter { get; set; }
        public string CategoryName { get; set; }
        public string Classification { get; set; }
        public string ItemName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; } 
        
        public decimal Rate { get; set; }
        public decimal Discount { get; set; }
        public decimal Value { get; set; }

    }
}
