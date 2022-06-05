using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class RestaurantItemBO
    {
        public int ItemId { get; set; }
        public int RandomItemId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal UnitPrice { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
