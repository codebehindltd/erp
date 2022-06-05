using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class RoomReservationInfoReportBO
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
        public int NoShowQuantity { get; set; }
        public string ReservedRoom { get; set; }
        public string NoShowRoomList { get; set; }
        public decimal? RoomRate { get; set; }
        public int? CurrencyTypeId { get; set; }
        public string CurrencyType { get; set; }
        public string PaymentMode { get; set; }
        public string GuestReferance { get; set; }
        public string ReservationMode { get; set; }
        public string Reason { get; set; }


    }
}
