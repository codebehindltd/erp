using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class WalkInGuestReportBO
    {
        public string RoomNumber { get; set; }
        public int? ReservationNo { get; set; }
        public string FolioNo { get; set; }
        public int? Pax { get; set; }
        public int Adult { get; set; }
        public int Child { get; set; }
        public int RegistrationId { get; set; }
        public string RegistrationNumber { get; set; }
        public string GuestName { get; set; }
        public string CompanyName { get; set; }
        public DateTime? ArriveDate { get; set; }
        public DateTime? ExpectedCheckOutDate { get; set; }
        public string ArriveDateString { get; set; }
        public string ExpectedCheckOutDateString { get; set; }
        public string GuestNationality { get; set; }
        public string UserName { get; set; }
        public string PL { get; set; }
    }
}
