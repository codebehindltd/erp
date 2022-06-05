using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestReservationMappingBO
    {
        public long GuestReservationId { get; set; }
        public long ReservationId { get; set; }
        public long GuestId { get; set; }
        public int RoomId { get; set; }
    }
}
