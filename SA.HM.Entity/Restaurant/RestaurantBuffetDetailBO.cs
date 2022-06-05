using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class RestaurantBuffetDetailBO
    {
        public int DetailId { get; set; }
        public int BuffetId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
