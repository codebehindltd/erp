using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestRefReportViewBO
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string RegistrationNumber { get; set; }
        public string GuestName { get; set; }
        public string ArriveDate { get; set; }
        public string CheckOutDate { get; set; }
        public decimal? SalesAmount { get; set; }
        public decimal? ReferanceComissionRate { get; set; }
        public decimal? SatyedNights { get; set; }
        public string RoomNumber { get; set; }
        public decimal RoomRent { get; set; }
    }
}
