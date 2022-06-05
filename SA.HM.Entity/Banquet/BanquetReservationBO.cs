using HotelManagement.Entity.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Banquet
{
    public class BanquetReservationBO
    {
        public long Id { get; set; }
        public long ReservationId { get; set; }
        public string ReservationNumber { get; set; }
        public long BanquetId { get; set; }
        public string ReservationMode { get; set; }
        public string Name { get; set; }
        public Boolean IsListedCompany { get; set; }
        public int CompanyId { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }
        public int CountryId { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string BookingFor { get; set; }
        public string ContactPerson { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public DateTime ArriveDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public long OccessionTypeId { get; set; }
        public long SeatingId { get; set; }
        public int NumberOfPersonAdult { get; set; }
        public int NumberOfPersonChild { get; set; }
        public string Address { get; set; }
        public string Remarks { get; set; }
        public long? RefferenceId { get; set; }
        public string CancellationReason { get; set; }
        public string SpecialInstructions { get; set; }
        public string Comments { get; set; }
        public decimal TotalAmount { get; set; }
        public string ReservationDiscountType { get; set; }
        public decimal ReservationDiscountAmount { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public long CreatedBy { get; set; }
        public long LastModifiedBy { get; set; }
        public bool IsReturnedClient { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public bool ActiveStatus { get; set; }
        public string BillStatus { get; set; }
        public string ReferenceName { get; set; }
        public int BillVoidBy { get; set; }
        public string BillVoidByName { get; set; }
        public string BillVoidDateTime { get; set; }
        public decimal InvoiceServiceRate { get; set; }
        public bool IsInvoiceServiceChargeEnable { get; set; }
        public decimal InvoiceServiceCharge { get; set; }
        public bool IsInvoiceCitySDChargeEnable { get; set; }
        public decimal InvoiceCitySDCharge { get; set; }
        public bool IsInvoiceVatAmountEnable { get; set; }
        public decimal InvoiceVatAmount { get; set; }        
        public bool IsInvoiceAdditionalChargeEnable { get; set; }
        public string AdditionalChargeType { get; set; }
        public decimal InvoiceAdditionalCharge { get; set; }
        public decimal BanquetRate { get; set; }
        public decimal ServiceRate { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal CitySDCharge { get; set; }
        public decimal VatAmount { get; set; }
        public decimal AdditionalCharge { get; set; }
        public bool IsBillSettlement { get; set; }
        public bool IsBillReSettlement { get; set; }
        public int CostCenterId { get; set; }
        public string RebateRemarks { get; set; }
        public int IsDayClosed { get; set; }

        //new 
        public int GLCompanyId { get; set; }
        public int GLProjectId { get; set; }
        public string EventType { get; set; }
        public string EventTitle { get; set; }
        public string MeetingAgenda { get; set; }
        public string MeetingDiscussion { get; set; }
        public string CallToAction { get; set; }
        public bool IsUnderCompany { get; set; }
        public string Venue { get; set; }
        public List<EmployeeBO> PerticipantFromOffice { get; set; }
        public List<EmployeeBO> PerticipantFromClient { get; set; }
        /// <summary>
        /// This property will only Use for banquet Reservation whereas the employees are selecting from the drop down list as like comma seperated ids.
        /// </summary>
        public string PerticipantFromOfficeCommaSeperatedIds { get; set; }
        public string PerticipantFromClientCommaSeperatedIds { get; set; }
    }
}
