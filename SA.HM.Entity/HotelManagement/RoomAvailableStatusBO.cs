using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class RoomAvailableStatusBO
    {
        public int TransectionOrderId { get; set; }
        public DateTime TransectionDate { get; set; }
        public string TransectionDateDisplay { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomType { get; set; }
        public int TotalRoomNumber { get; set; }
        public int TotalAvailable { get; set; }
        public int OutOfOrderAndOutOfServiceRoomCount { get; set; }
}
}
