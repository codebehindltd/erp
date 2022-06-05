using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class RestaurantKitchenCostCenterMappingBO
    {
        public long MappingId { get; set; }
        public int CostCenterId { get; set; }
        public int KitchenId { get; set; }
    }
}
