using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class RestaurantKitchenBO
    {
        public int KitchenId { get; set; }
        public int CostCenterId { get; set; }
        public int KotId { get; set; }
        public string CostCenter { get; set; }
        public string KitchenName { get; set; }
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }

        public int IsCanDeliver { get; set; }
    }
}
