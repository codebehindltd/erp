using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class SBReportViewBO
    {
        public string HotelCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CountryName { get; set; }
        public string GuestCompany { get; set; }
        public string PassportNumber { get; set; }
        public string GuestSex { get; set; }
        public string GuestDOB { get; set; }
        public string Profession { get; set; }
        public string CheckInDate { get; set; }
        public string CheckOutDate { get; set; }
        public string BookingRefference { get; set; }
        public string RoomNumber { get; set; }  
        public string GuestName { get; set; }
        public string GuestFather { get; set; }
        public string GuestAddress { get; set; }
        public string GuestNationality { get; set; }
        public string ProfessionName { get; set; }
        public string VisitPurpose { get; set; }
        public string Identification { get; set; }
        public string GuestMobile { get; set; }
        public string Remarks { get; set; }
        public int SortingOrderByArrivalNChekOutDate { get; set; }
    }
}
