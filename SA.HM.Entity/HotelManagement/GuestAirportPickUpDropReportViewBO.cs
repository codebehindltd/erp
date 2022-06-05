using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestAirportPickUpDropReportViewBO
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string TransactionNumber { get; set; }
        public string GuestName { get; set; }
        public string FlightName { get; set; }
        public string FlightNumber { get; set; }
        public string TimeString { get; set; }
        public string RoomNumber { get; set; }
    }
}
