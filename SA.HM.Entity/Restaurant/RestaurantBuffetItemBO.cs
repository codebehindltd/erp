using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class RestaurantBuffetItemBO
    {
        public int BuffetId { get; set; }
        public int RandomBuffetId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int CostCenterId { get; set; }
        public string BuffetName { get; set; }
        public decimal BuffetPrice { get; set; }
        public string Code { get; set; }
        public string ImageName { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
