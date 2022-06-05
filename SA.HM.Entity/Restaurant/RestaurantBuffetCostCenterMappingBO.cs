using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class RestaurantBuffetCostCenterMappingBO
    {
        public int MappingId { get; set; }
        public int CostCenterId { get; set; }
        public int BuffetId { get; set; }
        public decimal MinimumStockLevel { get; set; }
        public decimal StockQuantity { get; set; }
        public decimal UnitPriceLocal { get; set; }
        public decimal UnitPriceUsd { get; set; }
    }
}
