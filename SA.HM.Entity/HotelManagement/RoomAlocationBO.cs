using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class RoomAlocationBO
    {
        public int RegistrationId { get; set; }
        public int ReservationId { get; set; }
        public string GuestName { get; set; }
        public string GuestCountry { get; set; }
        public string GuestPassport { get; set; }
        public string RoomType  { get; set; }
        public string RegistrationNumber { get; set; }
        public decimal RoomRate { get; set; }
        public int RoomTypeId { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public int CompanyId { get; set; }
        public int AccountHeadCompanyId { get; set; }
        public string CompanyName { get; set; }
        public int NodeId { get; set; }
        public int CurrencyType { get; set; }
        public string LocalCurrencyHead { get; set; }
        public string CurrencyTypeHead { get; set; }
        public decimal ConversionRate { get; set; }
        public DateTime ArriveDate { get; set; }
        public DateTime BillingStartDate { get; set; }
        public DateTime ExpectedCheckOutDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal UnitPrice { get; set; }
        public int BusinessPromotionId { get; set; }
        public Boolean IsCompanyGuest { get; set; }
        public Boolean IsHouseUseRoom { get; set; }
        public Boolean IsStopChargePosting { get; set; }
        public string Remarks { get; set; }
        public long MasterId { get; set; }
        public string RoomName { get; set; }
        public decimal MinimumRoomRate { get; set; }
    }
}
