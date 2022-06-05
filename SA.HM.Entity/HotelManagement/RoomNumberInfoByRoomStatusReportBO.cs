using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity
{
    public class RoomNumberInfoByRoomStatusReportBO
    {
        public int RoomId { get; set; }
        public int? RoomTypeId { get; set; }
        public string RoomType { get; set; }
        public string RoomNumber { get; set; }
        public int? StatusId { get; set; }
        public string ActiveStatus { get; set; }
        public string CSSClassName { get; set; }
    }
}
