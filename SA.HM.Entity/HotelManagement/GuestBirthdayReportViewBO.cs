using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestBirthdayReportViewBO
    {
        public string ViewDate { get; set; }
        public string GuestName { get; set; }
        public string GuestDOB { get; set; }
        public string CompanyName { get; set; }
        public string CountryName { get; set; }
        public string GuestPhone { get; set; }
        public string GuestEmail { get; set; }
        public int SatyedNights { get; set; }
    }
}
