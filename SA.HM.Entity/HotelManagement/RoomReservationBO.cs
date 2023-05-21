using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class RoomReservationBO
    {
        public long Id { get; set; }
        public int PaymentId { get; set; }
        public int ReservationId { get; set; }
        public int PickUpDropCount { get; set; }
        public int ReservationTempId { get; set; }
        public int OnlineReservationId { get; set; }
        public string ReservationNumber { get; set; }
        public DateTime ReservationDate { get; set; }
        public string ReservationDateDisplay { get; set; }
        public DateTime DateIn { get; set; }        
        public string DateInDisplay { get; set; }        
        public DateTime DateOut { get; set; }
        public string DateOutDisplay { get; set; }
        public string InvoiceNo { get; set; }
        public string ReceivedBy { get; set; }
        public DateTime ProbableArrivalTime { get; set; }
        public DateTime ProbableDepartureTime { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public string ReservedCompany { get; set; }
        public int? GuestId { get; set; }
        public string GuestName { get; set; }
        public string GuestEmail { get; set; }
        public string GuestPhone { get; set; }
        public string ContactAddress { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string MobileNumber { get; set; }
        public string FaxNumber { get; set; }
        public string ContactEmail { get; set; }
        public int TotalRoomNumber { get; set; }
        public string ReservedMode { get; set; }
        public string ReservationType { get; set; }
        public string ReservationMode { get; set; }
        public string Reason { get; set; }
        public bool IsListedCompany { get; set; }
        public bool IsListedContact { get; set; }
        public long ContactId { get; set; }
        public int NumberOfPersonAdult { get; set; }
        public int NumberOfPersonChild { get; set; }
        public Boolean IsFamilyOrCouple { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public int BusinessPromotionId { get; set; }
        public int ReferenceId { get; set; }
        public string PaymentMode { get; set; }
        public int PayFor { get; set; }
        public int CurrencyType { get; set; }
        public string CurrencyName { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal CurrencyAmount { get; set; }
        public decimal ConversionRate { get; set; }
        public int IsAirportPickupDropExist { get; set; }
        public int APDId { get; set; }
        public string ArrivalFlightName { get; set; }
        public string ArrivalFlightNumber { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int DepartureAirlineId { get; set; }
        public string DepartureFlightName { get; set; }
        public string DepartureFlightNumber { get; set; }
        public DateTime DepartureTime { get; set; }
        public string DepartureTimeString { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public string Remarks { get; set; }
        public string GuestRemarks { get; set; }
        public string POSRemarks { get; set; } 
        public string AirportPickUp { get; set; }
        public string AirportDrop { get; set; }
        public string RoomInformation { get; set; }
        public string DateInFieldEdit { get; set; }
        public string MinCheckInDate { get; set; }
        public string DateInStr { get; set; }
        public string ReservationNDetailNRoomId { get; set; }
        public int ReservationDetailId { get; set; }
        public string Status { get; set; }
        public string ShowDateIn { get; set; }

        ////-------------------For Express Check In
        public string RoomReservationGrid { get; set; }
        public string ExpressCheckInnDetailsGrid { get; set; }
        public string ExpressCheckInnCalenderDetailsGrid { get; set; }
        public string ReservationDetailGrid { get; set; }
        public int RoomId { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomType { get; set; }
        public string RoomTypeCode { get; set; }
        public string RoomNumber { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal RoomRate { get; set; }
        public int IsRegistered { get; set; }
        public string DateInString { get; set; }
        public string DateOutString { get; set; }
        public int MarketSegmentId { get; set; }
        public int GuestSourceId { get; set; }
        public string RoomInfo { get; set; }
        public int TotalPaxQuantity { get; set; }
        public int PaxQuantity { get; set; }
        public int RoomQuantity { get; set; }
        public bool IsRoomRateShowInPreRegistrationCard { get; set; }
        public List<RoomAssignDuplicationCheckVwBO> DuplicateCheck { get; set; }
        public int MealPlanId { get; set; }
        public bool IsVIPGuest { get; set; }
        public int VipGuestTypeId { get; set; }
        public bool IsComplementaryGuest { get; set; }        
        public int ClassificationId { get; set; }
        public string BookersName { get; set; }
        public string TransactionType { get; set; }
        public int TypeWiseRoomQuantity { get; set; }
        public bool IsServiceChargeEnable { get; set; }
        public bool IsCityChargeEnable { get; set; }
        public bool IsVatAmountEnable { get; set; }
        public bool IsAdditionalChargeEnable { get; set; }
        public string ReservationDetails { get; set; }
        public string GroupName { get; set; }
        public string CheckInDateDisplay { get; set; }
        public string CheckOutDateDisplay { get; set; }
        public string GroupDescription { get; set; }
        public Boolean IsTaggedOnGroupReservation { get; set; }
    }
}
