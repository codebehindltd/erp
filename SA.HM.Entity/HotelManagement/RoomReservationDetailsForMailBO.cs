using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class RoomReservationDetailsForMailBO
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string WebAddress { get; set; }
        public string GuestName { get; set; }
        public string GuestCompanyName { get; set; }
        public string GuestCompanyAddress { get; set; }
        public string ContactPerson { get; set; }
        public string ContactDesignation { get; set; }
        public string TelephoneNumber { get; set; }
        public string ContactNumber { get; set; }
        public string FaxNumber { get; set; }
        public string ContactEmail { get; set; }
        public string ReservationNumber { get; set; }
        public string ReferencePerson { get; set; }
        public string ReferenceDesignation { get; set; }
        public string ReferenceOrganization { get; set; }
        public string ReferenceTelephone { get; set; }
        public string ReferenceCellNumber { get; set; }
        public string ReferenceEmail { get; set; }
        public string MethodOfPayment { get; set; }
        public string RoomType { get; set; }
        public string ArrivalFlightName { get; set; }
        public string ArrivalFlightNumber { get; set; }
        public string CurrencyType { get; set; }
        public string DepartureFlightName { get; set; }
        public string DepartureFlightNumber { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal RoomRate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ReservationDate { get; set; }

        public decimal TypeWiseTotalRooms { get; set; }

        public decimal TotalNumberOfRooms { get; set; }
        public decimal ConversionRate { get; set; }


    }
}
