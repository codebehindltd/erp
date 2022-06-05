using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
   public class RestaurantBillDetailBO
    {
        public int DetailId { get; set; }        
        public int BillId { get; set; }
        public int KotId { get; set; }

        public int MainBillId { get; set; }
        public int TableId { get; set; }
    }
}
