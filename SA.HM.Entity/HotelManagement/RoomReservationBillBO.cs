using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class RoomReservationBillBO
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string WebAddress { get; set; }
        public string GuestName { get; set; }
        public string GuestList { get; set; }
        public Nullable<int> TotalGuest { get; set; }
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
        public string ReservedMode { get; set; }
        public string PaymentMode { get; set; }
        public string MethodOfPayment { get; set; }
        public string RoomType { get; set; }
        public int RoomTypeWisePaxQuantity { get; set; }
        public Nullable<int> TypeWiseTotalRooms { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> RoomRate { get; set; }
        public Nullable<decimal> TotalNumberOfRooms { get; set; }
        public string LocalCurrencyType { get; set; }
        public Nullable<int> CurrencyTypeId { get; set; }
        public string CurrencyType { get; set; }
        public string AirportPickUp { get; set; }
        public decimal ConversionRate { get; set; }
        public string ArrivalFlightName { get; set; }
        public string ArrivalFlightNumber { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string StringArrivalDate { get; set; }
        public TimeSpan? ArrivalTime { get; set; }
        public Nullable<System.DateTime> ArrivalDateTime { get; set; }
        public string AirportDrop { get; set; }
        public string DepartureFlightName { get; set; }
        public string DepartureFlightNumber { get; set; }
        public DateTime DepartureDate { get; set; }
        public string StringDepartureDate { get; set; }
        public TimeSpan? DepartureTime { get; set; }
        public Nullable<int> RoomOfNights { get; set; }
        public Nullable<System.DateTime> DepartureDateTime { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> APDId { get; set; }
        public string ReservationMode { get; set; }
        public Boolean IsRoomRateShowInPreRegistrationCard { get; set; }
        public int IsOtherChargeEnabled { get; set; }
        public string CompanyDetails { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public string CheckInDateDisplay { get; set; }
        public string CheckOutDateDisplay { get; set; }
        public string ModeOfPayment { get; set; }
        public string BillingAddress { get; set; }
        public string CreatedDateDisplay { get; set; }
        public string GroupReservationNumber { get; set; }        
    }
}
