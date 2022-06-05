using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
   public class RestaurantTableBO
    {
        public int TableId { get; set; }
        public string TableNumber { get; set; }
        public string TableCapacity { get; set; }
        public int StatusId { get; set; }

        public string Status { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set;}
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
    }
}
