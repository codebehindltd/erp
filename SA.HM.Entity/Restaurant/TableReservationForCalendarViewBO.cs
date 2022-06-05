using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class TableReservationForCalendarViewBO
    {
        public int TableId { get; set; }
        public int ArriveHour { get; set; }
        public int DepartHour { get; set; }
        public int ReservationId { get; set; }

        public string ContactPerson { get; set; }
    }
}
