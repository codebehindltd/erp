using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class ComplimentaryGuestReportViewBO
    {
        public DateTime CurrentDate { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public string GuestName { get; set; }
        public string CompanyName { get; set; }
        public string RoomNumber { get; set; }
        public string Remarks { get; set; }
    }
}
