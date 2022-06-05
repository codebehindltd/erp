using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestHouseCheckOutDetailBO
    {
        public long ServiceBillId { get; set; }
        public int RegistrationId { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public string GuestName { get; set; }
        public int RoomNumber { get; set; } 
        public DateTime ServiceDate { get; set; }
        public int ServiceId { get; set; }
        public string ServiceType { get; set; }
        public string CostCenter { get; set; } 
        public string ServiceName { get; set; }
        public decimal ServiceRate { get; set; }
        public decimal ServiceQuantity { get; set; }
        public decimal VatAmount { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal CitySDCharge { get; set; }
        public decimal AdditionalCharge { get; set; }
        public decimal ReferenceSalesCommission { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal UsdTotalAmount { get; set; }
        public string NightAuditApproved { get; set; }
        public decimal VatAmountPercent { get; set; }
        public decimal ServiceChargePercent { get; set; }
        public decimal CalculatedPercentAmount { get; set; }
        public decimal CurrencyExchangeRate { get; set; }        
        public int IncomeNodeId { get; set; }
        public Boolean IsPaidService { get; set; }
        public Boolean IsPaidServiceAchieved { get; set; }
        public int IsGuestTodaysBillAdd { get; set; }
        public int BillPaidBy { get; set; }
        public int BillPaidByRoomNumber { get; set; }        
    }
}
