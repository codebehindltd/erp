using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class RoomCalenderBO
    {
        public int TransectionId { get; set; }
        public string RoomType { get; set; }
        public int RoomTypeId { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public DateTime OriginalCheckOutDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransectionStatus { get; set; }
        public string GuestName { get; set; }
        public string RoomInformation { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceType { get; set; }
        public decimal ServiceQuantity { get; set; }
        public string DisplayQuantity { get; set; }
        public string ColorCodeName { get; set; }
        public string ActiveStatus { get; set; }
    }
}
