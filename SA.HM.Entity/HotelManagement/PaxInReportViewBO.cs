using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class PaxInReportViewBO
    {
        public string RegistrationNumber { get; set; }
        public string GuestName { get; set; }
        public string RoomNumber { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public Decimal PaxInRate { get; set; }
        public string CurrencyName { get; set; }
    }
}
