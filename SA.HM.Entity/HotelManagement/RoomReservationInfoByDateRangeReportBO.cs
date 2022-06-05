using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class RoomReservationInfoByDateRangeReportBO
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string ReservationNumber { get; set; }
        public string BookingDate { get; set; }
        public string CheckIn { get; set; }
        public string ProbableCheckInTime { get; set; }
        public string DateOut { get; set; }
        public string GuestName { get; set; }
        public string GuestList { get; set; }
        public int? TotalPerson { get; set; }
        public string ReservedBy { get; set; }
        public string CompanyORPrivate { get; set; }
        public string RoomType { get; set; }
        public int? Quantity { get; set; }
        public string RoomNumberList { get; set; }
        public decimal? RoomRate { get; set; }
        public int? CurrencyTypeId { get; set; }
        public string CurrencyType { get; set; }
        public string PaymentMode { get; set; }
        public string GuestReferance { get; set; }
        public string ReservationMode { get; set; }
        public string Reason { get; set; }
        public Nullable<int> ReservationId { get; set; }
        public Nullable<int> ReservationDetailId { get; set; }
        public Nullable<int> RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string ReservationNumberNGuestName { get; set; }
        public Nullable<int> NoOfNights { get; set; }
        public DateTime RCheckInDate { get; set; }
        public DateTime RCheckOutDate { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public Nullable<int> PartialRegistration { get; set; }
        public decimal ReservationAdvanceTotal { get; set; }
        public string ReservationDescription { get; set; }
    }
}
