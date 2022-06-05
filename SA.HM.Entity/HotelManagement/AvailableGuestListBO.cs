using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class AvailableGuestListBO
    {
        public int ApprovedId { get; set; }
        public int RegistrationId { get; set; }
        public DateTime ServiceDate { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string GuestName { get; set; }
        public string RoomType { get; set; }
        public int RoomId { get; set; }
        public int RoomNumber { get; set; }
        public string ServiceName { get; set; }
        public decimal PreviousRoomRate { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal RoomRate { get; set; }
        public decimal BPPercentAmount { get; set; }
        public decimal BPDiscountAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal CitySDCharge { get; set; }
        public decimal AdditionalCharge { get; set; }
        public decimal VatAmountPercent { get; set; }
        public decimal ServiceChargePercent { get; set; }
        public decimal ReferenceSalesCommission { get; set; }
        public decimal ReferenceSalesCommissionPercent { get; set; }
        public decimal CalculatedPercentAmount { get; set; }
        public decimal TotalCalculatedAmount { get; set; }
        public decimal TotalCalculatedUsdAmount { get; set; }
        public Boolean ApprovedStatus { get; set; }
        public int IsRoomOwner { get; set; }
        public decimal CalculatedRoomRate { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public int IsBillInclusive { get; set; }
        public decimal GuestServiceChargeRate { get; set; }
        public decimal GuestVatAmountRate { get; set; }
        public int InvoiceServiceCharge { get; set; }
        public int IsGuestCheckedOut { get; set; }
        public int IsServiceChargeEnable { get; set; }
        public int IsCitySDChargeEnable { get; set; }
        public int IsVatAmountEnable { get; set; }
        public int IsAdditionalChargeEnable { get; set; }


        public decimal ServiceChargeConfig { get; set; }
        public decimal CitySDChargeConfig { get; set; }
        public decimal VatAmountConfig { get; set; }
        public string AdditionalChargeTypeConfig { get; set; }
        public decimal AdditionalChargeConfig { get; set; }
        public int IsDiscountApplicableOnRackRateConfig { get; set; }
        public int IsVatOnSDChargeConfig { get; set; }
        public int IsRatePlusPlusConfig { get; set; }
        public string ApprovedServiceType { get; set; }
    }
}
