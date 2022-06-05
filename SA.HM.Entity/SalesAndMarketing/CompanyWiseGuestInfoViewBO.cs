using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class CompanyWiseGuestInfoViewBO
    {
        public string RegistrationNumber { get; set; }
        public string GuestName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string EmailAddress { get; set; }
        public string WebAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string CompanyRemarks { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string GuestNationality { get; set; }
        public string PassportNumber { get; set; }
        public string GuestDOB { get; set; }
        public string GuestAge { get; set; }
        public int TotalPerson { get; set; }
        public string DateIn { get; set; }
        public string DateOut { get; set; }
        public int ReferenceId { get; set; }
        public string GuestReferance { get; set; }
        public int CurrencyType { get; set; }
        public string CurrencyHead { get; set; }
        public string RoomType { get; set; }
        public string RoomNumber { get; set; }
        public decimal RoomRate { get; set; }
        public decimal NoOfNight { get; set; }
        public decimal TotalBill { get; set; }
        public string AirportPickUp { get; set; }
        public string AirportDrop { get; set; }
        public string ArrivalFlightName { get; set; }
        public string ArrivalFlightNumber { get; set; }
        public string ArrivalTimeString { get; set; }
        public string UserName { get; set; }
        public string CheckOutBy { get; set; }
        public string Remarks { get; set; }        
    }
}
