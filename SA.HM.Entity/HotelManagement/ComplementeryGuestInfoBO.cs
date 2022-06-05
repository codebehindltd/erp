using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
     public class ComplementeryGuestInfoBO
    {
        public int? RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string FollioNo { get; set; }
        public string ComplementaryRemarks { get; set; }
        public int? Pax { get; set; }
        public int Adult { get; set; }
        public int Child { get; set; }
        public int RegistrationId { get; set; }
        public string RegistrationNumber { get; set; }
        public string GuestName { get; set; }
        public string Status { get; set; }
        public string GuestNationality { get; set; }
        public DateTime? ArriveDate { get; set; }
        public DateTime? ExpectedCheckOutDate { get; set; }
        public string ArriveDateString { get; set; }
        public string ExpectedCheckOutDateString { get; set; }
        public string PaymentMode { get; set; }
        public string BillInstruction { get; set; }
        public string BusinessSource { get; set; }
        public string MarketSegment { get; set; }
        public string UserName { get; set; }
        public string GuestCompany { get; set; }
    }
}
