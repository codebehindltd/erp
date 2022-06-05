using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class ReservationRoomDetailsBO : ReservationDetailBO
    {
        int RoomDetailsId { get; set; }
        List<RoomNumberBO> RoomNumberReservedList { get; set; }
    }
}
