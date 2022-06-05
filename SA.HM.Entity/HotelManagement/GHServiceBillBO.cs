using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GHServiceBillBO
    {
        public int CostCenterId { get; set; }
        public int ApprovedId { get; set; }
        public int ServiceBillId { get; set; }
        public DateTime ApprovedDate { get; set; }
        public DateTime ServiceDate { get; set; }
        public string BillNumber { get; set; }
        public int RegistrationId { get; set; }
        public string RegistrationNumber { get; set; }
        public string GuestName { get; set; }
        public string ServiceType { get; set; }
        public string GuestType { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public decimal ServiceRate { get; set; }
        public decimal ServiceQuantity { get; set; }
        public decimal DiscountAmount { get; set; }
        public int NodeId { get; set; }
        public bool IsServiceChargeEnable { get; set; }
        public bool IsVatAmountEnable { get; set; }
        public bool IsCitySDChargeEnable { get; set; }
        public bool IsAdditionalChargeEnable { get; set; }
        public decimal RackRate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string PaymentMode { get; set; }
        public int FieldId { get; set; }
        public decimal CurrencyAmount { get; set; }
        public decimal PaymentAmout { get; set; }
        public int BankId { get; set; }
        public int DealId { get; set; }
        public string BranchName { get; set; }
        public string ChecqueNumber { get; set; }
        public DateTime ChecqueDate { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public DateTime? CardExpireDate { get; set; }
        public string CardHolderName { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int? EmpId { get; set; }
        public int? CompanyId { get; set; }
        public decimal VatAmount { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal CitySDCharge { get; set; }
        public decimal AdditionalCharge { get; set; }
        public decimal ReferenceSalesCommission { get; set; }
        public decimal VatAmountPercent { get; set; }
        public decimal ServiceChargePercent { get; set; }
        public decimal CalculatedPercentAmount { get; set; }
        public decimal TotalCalculatedAmount { get; set; }
        public decimal ConversionRate { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public string RoomNumber { get; set; }
        public Boolean ApprovedStatus { get; set; }
        public Boolean IsComplementary { get; set; }
        public Boolean IsPaidService { get; set; }
        public Boolean IsPaidServiceAchieved { get; set; }
        public string Remarks { get; set; }
        public string Reference { get; set; }
        public int BillEdditableStatus { get; set; }
        public string ModuleName { get; set; }
        public int RestaurantBillId { get; set; }
        public Boolean IsBillHoldUp { get; set; }
    }
}
