using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestCheckOutViewBO
    {
        public int Id { get; set; }
        public string GuestName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public string GuestPhone { get; set; }
        public decimal PaxInRate { get; set; }
    }
}
