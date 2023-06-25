using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class RoomRegistrationBO
    {
        public int RegistrationId { get; set; }
        public int TmpRegistrationId { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime OriginalArriveDate { get; set; }
        public string OriginalArriveDateDisplay { get; set; }
        public DateTime ArriveDate { get; set; }
        public DateTime? BillingStartDate { get; set; }
        public DateTime? ActualCheckOutDate { get; set; }
        public DateTime ExpectedCheckOutDate { get; set; }
        public Boolean IsExpectedCheckOutTimeEnable { get; set; }
        public DateTime ProbableDepartureTime { get; set; }
        public DateTime? BillHoldUpDate { get; set; }
        public int RoomId { get; set; }
        public int RoomTypeId { get; set; }
        public int RoomNumber { get; set; }
        public int EntitleRoomType { get; set; }
        public Boolean IsCompanyGuest { get; set; }
        public Boolean IsHouseUseRoom { get; set; }
        public Boolean IsFamilyOrCouple { get; set; }
        public int NumberOfPersonAdult { get; set; }
        public int NumberOfPersonChild { get; set; }
        public string CommingFrom { get; set; }
        public string NextDestination { get; set; }
        public string VisitPurpose { get; set; }
        public Boolean IsFromReservation { get; set; }
        public int ReservationId { get; set; }
        public string ReservationInfo { get; set; }
        public bool IsListedCompany { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string PaymentMode { get; set; }
        public int PayFor { get; set; }
        public string ReservedCompany { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public int BusinessPromotionId { get; set; }
        public int IsRoomOwner { get; set; }
        public int CreatedBy { get; set; }
        public int MealPlanId { get; set; }
        public int ReferenceId { get; set; }
        public int LastModifiedBy { get; set; }
        public int GuestSourceId { get; set; }
        public bool IsReturnedGuest { get; set; }
        public bool IsVIPGuest { get; set; }
        public int VIPGuestTypeId { get; set; }
        public string RegistrationMode { get; set; }
        public int CurrencyType { get; set; }
        public string Currency { get; set; }
        public string LocalCurrencyHead { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal RoomRate { get; set; }
        public decimal TotalRoomRate { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountAmount { get; set; } 
        public string AirportPickUp { get; set; }
        public string AirportDrop { get; set; }
        public int IsAirportPickupDropExist { get; set; }
        public int APDId { get; set; }               
        public string ArrivalFlightName { get; set; }
        public string ArrivalFlightNumber { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int DepartureAirlineId { get; set; }
        public string DepartureFlightName { get; set; }
        public string DepartureFlightNumber { get; set; }
        public DateTime? DepartureTime { get; set; }
        public Boolean IsDepartureChargable { get; set; }

        public int GuestId { get; set; }
        public string GuestName { get; set; }
        public string CountryName { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string CheckOutDateDisplay { get; set; }
        public int IsGuestCheckedOut { get; set; }
        public bool IsRoomNumberCheckoutOrRegistationAfterCurrentGuestCheckOut { get; set; }
        public decimal PaxInRate { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal VatAmount { get; set; }
        public decimal RackRate { get; set; }
        public int IsInclusive { get; set; }
        public bool IsCityChargeEnable { get; set; }
        public bool IsServiceChargeEnable { get; set; }
        public bool IsVatAmountEnable { get; set; }
        public bool IsAdditionalChargeEnable { get; set; }        
        public bool IsPaidServiceExist { get; set; }
        public string Remarks { get; set; }
        public string GuestRemarks { get; set; }
        public string POSRemarks { get; set; } 
        public int BillPaidByRegistrationId { get; set; }
        public string BillPaidByRegistrationNumber { get; set; }
        public string BillPaidByRoomNumber { get; set; }
        public string RoomNo { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime? CardExpireDate { get; set; }
        public string CardReference { get; set; }
        public string CardExpireDateShow { get; set; }
        public Boolean IsBillHoldUp { get; set; }
        public Boolean IsStopChargePosting { get; set; }
        public Boolean IsBlankRegistrationCard { get; set; }

        public bool IsEarlyCheckInChargeEnable { get; set; }
        public decimal HoldUpAmount { get; set; }
        public virtual Guid GuidId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CheckOutDateForAPI { get; set; }
        public Int64 ReservationDetailId { get; set; }

        public int? MarketSegmentId { get; set; }

        public bool IsListedContact { get; set; }
        public long ContactId { get; set; }
        public long? PackageId { get; set; }
    }
}
