using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class RoomStatisticsInfoBO
    {
        public string ExpectedArrival { get; set; }
        public string ExpectedDeparture { get; set; }
        public string CheckInRoom { get; set; }
        public string WalkInRoom { get; set; }
        public string RoomToSell { get; set; }
        public string RegisterComplaint { get; set; }
        public string InhouseRoomOrGuest { get; set; }
        public string ExtraAdultsOrChild { get; set; }
        public string InhouseForeigners { get; set; }
        public string GuestBlock { get; set; }
        public string AirportPickupDrop { get; set; }
        public string Occupency { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
