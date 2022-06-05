using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Banquet
{
    public class BanquetReservationForCalendarViewBO
    {
        public long BanquetId { get; set; }
        public long ReservationId { get; set; }

        public int ArriveHour { get; set; }
        public int DepartureHour { get; set; }
        public string GuestName { get; set; }
        public Boolean IsBillSettlement { get; set; }
    }
}
