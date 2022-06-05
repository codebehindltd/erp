using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class HRPaidServiceViewBO
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string ServiceType { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
