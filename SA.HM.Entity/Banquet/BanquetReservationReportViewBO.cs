using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Banquet
{
    public class BanquetReservationReportViewBO
    {
        public DateTime ReservationDate { get; set; }
        public string BanquetName { get; set; }
        public string ReservationNo { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string BookingDate { get; set; }
        public string PartyStartDate { get; set; }
        public string PartyEndDate { get; set; }
        public int NoOfPersonAdult { get; set; }
        public int NoOfPersonChild { get; set; }
        public string Reference { get; set; }
        public string Status { get; set; }
    }
}
