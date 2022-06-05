using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestStayedNightBO
    {
        public string GuestName { get; set; }
        public string GuestPhone { get; set; }
        public string GuestEmail { get; set; }
        public string DOB { get; set; }
        public string CompanyName { get; set; }
        public string PassportNumber { get; set; }
        public int? SatyedNights { get; set; }
    }
}
