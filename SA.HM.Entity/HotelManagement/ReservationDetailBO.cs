using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class ReservationDetailBO
    {
        public int ReservationDetailId { get; set; }
        public int ReservationId { get; set; }
        public string ReservationNumber { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomType { get; set; }
        public string RoomTypeCode { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string DirtyRoomNumber { get; set; }
        public int NumberOfRoom { get; set; }
        public decimal RoomRate { get; set; }
        public decimal Discount { get; set; }
        public bool IsRegistered { get; set; }
        public int ReferenceId { get; set; }
        public int CurrencyType { get; set; }
        public string CurrencyHead { get; set; }
        public string DiscountType { get; set; }
        public string Status { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsCityChargeEnable { get; set; }
        public bool IsServiceChargeEnable { get; set; }
        public bool IsVatAmountEnable { get; set; }
        public bool IsAdditionalChargeEnable { get; set; }
        public int RoomQuantity { get; set; }
        public int PaxQuantity { get; set; }
        public int ChildQuantity { get; set; }
        public string GuestNotes { get; set; }
        public int ExtraBedQuantity { get; set; }
        public int RoomTypeWisePaxQuantity { get; set; }
        public string RoomNumberIdList { get; set; }
        public string RoomNumberList { get; set; }
        public string RoomNumberListInfoWithCount { get; set; }
        public string ReservationDetailIdList { get; set; }
        public decimal TotalCalculatedAmount { get; set; }
        public string RoomRateInfo { get; set; }
        public int TotalRoom { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ConversionRate { get; set; }
        public int IsUpdateDetailData { get; set; }
        public bool IsEditDetailData { get; set; }
        public int GuestId { get; set; }
        public int LoadType { get; set; }
        public string ReservationNDetailNRoomId { get; set; }
        public decimal NoShowCharge { get; set; }
        public int ExpressCheckInnRoomNumber { get; set; }
        public string ExpressCheckInnGuestName { get; set; }
        public string ArrivalDate { get; set; }
        public string DepartureDate { get; set; }

    }
}
