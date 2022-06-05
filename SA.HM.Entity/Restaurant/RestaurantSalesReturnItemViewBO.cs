using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class RestaurantSalesReturnItemViewBO: RestaurantSalesReturnItemBO
    {
        public String StockBy { get; set; }
        public decimal RemainingQuantity { get; set; }
        public int LocationId { get; set; }
    }
}
