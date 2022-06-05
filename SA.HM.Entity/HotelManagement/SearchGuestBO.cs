using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class SearchGuestBO
    {
        public int RegistrationId { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime ArriveDate { get; set; }
        public DateTime ExpectedCheckOutDate { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public int EntitleRoomType { get; set; }
        public Boolean IsCompanyGuest { get; set; }
        public int NumberOfPersonAdult { get; set; }
        public int NumberOfPersonChild { get; set; }
        public string CommingFrom { get; set; }
        public string NextDestination { get; set; }
        public string VisitPurpose { get; set; }
        public Boolean IsFromReservation { get; set; }
        public int ReservationId { get; set; }

        public int RegistrationDetailId { get; set; }
        public string GuestName { get; set; }
        public string GuestDOB { get; set; }
        public string GuestSex { get; set; }
        public string GuestEmail { get; set; }
        public string GuestPhone { get; set; }
        public string GuestAddress1 { get; set; }
        public string GuestAddress2 { get; set; }
        public string GuestCity { get; set; }
        public string GuestZipCode { get; set; }
        public string GuestCountryId { get; set; }
        public string GuestNationality { get; set; }
        public string GuestDrivinlgLicense { get; set; }
        public string GuestAuthentication { get; set; }
        public string NationalId { get; set; }
        public string PassportNumber { get; set; }
        public string PIssueDate { get; set; }
        public string PIssuePlace { get; set; }
        public string PExpireDate { get; set; }
        public string VisaNumber { get; set; }
        public string VIssueDate { get; set; }
        public string VExpireDate { get; set; }
    }
}
