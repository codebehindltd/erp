using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class AdvReseservationForecastViewBO
    {
        public string ReservationDate { get; set; }
        public DateTime? DateIn { get; set; }
        public DateTime? DateOut { get; set; }
        public string ReservationNo { get; set; }
        public string RoomNumberList { get; set; }
        public int? OccupiedRoomNo { get; set; }
        public decimal? Occupancy { get; set; }
        public string CompanyName { get; set; }
        public int? TotalNight { get; set; }
        public decimal? TotalRevenue { get; set; }
    }
}
