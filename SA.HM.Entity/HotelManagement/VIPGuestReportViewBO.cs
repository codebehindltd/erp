using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class VIPGuestReportViewBO
    {
        public string CheckInDate { get; set; }
        public string RoomNumber { get; set; }
        public string GuestName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string CheckOutDate { get; set; }
        public string CountryName { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string VIPGuestType { get; set; }
    }
}
