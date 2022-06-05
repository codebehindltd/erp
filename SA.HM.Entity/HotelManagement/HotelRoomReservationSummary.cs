using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    
    public partial class HotelRoomReservationSummary
    {
        public long Id { get; set; }

        public long? ReservationId { get; set; }

        public int? RoomTypeId { get; set; }

        public int? RoomQuantity { get; set; }

        public int? PaxQuantity { get; set; }
    }
}
