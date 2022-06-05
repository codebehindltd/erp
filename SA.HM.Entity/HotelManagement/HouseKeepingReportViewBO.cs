using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class HouseKeepingReportViewBO
    {
        public int? RoomId { get; set; }
        public int? RoomTypeId { get; set; }
        public int? RoomNumber { get; set; }
        public int? TotalInhouseGuest { get; set; }
        public int? StatusId { get; set; }
        public string ActiveStatus { get; set; }
        public string CleanupStatus { get; set; }
        public DateTime? CleanDate { get; set; }
        public DateTime? LastCleanDate { get; set; }
        public string Remarks { get; set; }
        public string StringCleanDate { get; set; }
        public string StringLastCleanDate { get; set; }

        public string RoomType { get; set; }
        public string FORoomStatus { get; set; }
        public string HKRoomStatus { get; set; }
        public string ReservationStatus { get; set; }
    }
}
