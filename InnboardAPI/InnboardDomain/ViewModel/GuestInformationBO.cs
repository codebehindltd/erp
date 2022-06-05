using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InnboardAPI.Models
{
    public class GuestInformationBO
    {
        public int GuestId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GuestName { get; set; }
        public DateTime? GuestDOB { get; set; }
        public string GuestDOBShow { get; set; }
        public string GuestSex { get; set; }
        public string GuestEmail { get; set; }
        public string GuestPhone { get; set; }
        public string GuestMobile { get; set; }
        public string GuestFax { get; set; }
        public string GuestAddress1 { get; set; }
        public string GuestAddress2 { get; set; }
        public int ProfessionId { get; set; }
        public string ProfessionName { get; set; }
        public string GuestCity { get; set; }
        public string GuestZipCode { get; set; }
        public int GuestCountryId { get; set; }
        public string GuestNationality { get; set; }
        public string GuestDrivinlgLicense { get; set; }
        public string GuestAuthentication { get; set; }
        public string NationalId { get; set; }
        public string PassportNumber { get; set; }
        public DateTime? PIssueDate { get; set; }
        public string PIssuePlace { get; set; }
        public DateTime? PExpireDate { get; set; }
        public string VisaNumber { get; set; }
        public DateTime? VIssueDate { get; set; }
        public string StrVIssueDate { get; set; }
        public DateTime? VExpireDate { get; set; }
        public string StrVExpireDate { get; set; }
        public string GuestPreferences { get; set; }
        public int? RoomId { get; set; }
        public string CountryName { get; set; }
        public int tempOwnerId { get; set; }
        public string Path { get; set; }
        public int RegistrationId { get; set; }
        public int NumberOfGuest { get; set; }
        public string RoomNumber { get; set; }
        public decimal RoomRate { get; set; }
        public decimal PaxInRate { get; set; }
        public string CurrencyTypeHead { get; set; }
        public DateTime ArriveDate { get; set; }
        public DateTime ExpectedCheckOutDate { get; set; }
        public int? TotalStayedNight { get; set; }
        public string ReservationMode { get; set; }
        public string ShowDOB { get; set; }
        public int PreviousGuestId { get; set; }
        public int APDId { get; set; }
        public string ArrivalFlightName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal EntRoomRate { get; set; }
        public string HMCompanyProfile { get; set; }
        public string HMCompanyAddress { get; set; }
        public string HMCompanyWeb { get; set; }
        public string RegistrationNumber { get; set; }
        public string CommingFrom { get; set; }
        public string NextDestination { get; set; }
        public string VisitPurpose { get; set; }
        public string RoomType { get; set; }
        public string CurrencyType { get; set; }
        public string EntRoomType { get; set; }
        public int CurrencyTypeId { get; set; }
        public int CompanyCountryId { get; set; }
        public int NumberOfPersonAdult { get; set; }
        public int NumberOfPersonChild { get; set; }
        public string ArriveDateString { get; set; }
        public string ExpectedCheckOutDateString { get; set; }
        public string GuestPreferenceId { get; set; }
        public string ReferanceName { get; set; }
        public string ShowCheckInDate { get; set; }
        public string ShowCheckOutDate { get; set; }
        public string ShowPIssueDate { get; set; }
        public string ShowPExpireDate { get; set; }
        public string ShowVIssueDate { get; set; }
        public string ShowVExpireDate { get; set; }
        public string Remarks { get; set; }
        public int ClassificationId { get; set; }
        public int KotId { get; set; }
        public int CostCenterId { get; set; }
        public bool IsStopChargePosting { get; set; }
        public string CompanyName { get; set; }
        public string MealPlan { get; set; }
    }
}