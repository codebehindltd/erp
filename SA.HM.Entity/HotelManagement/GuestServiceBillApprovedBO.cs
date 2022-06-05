using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestServiceBillApprovedBO
    {
        public long ApprovedId { get; set; }
        public int RegistrationId { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int ServiceBillId { get; set; }
        //public DateTime ServiceDate { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceInfo { get; set; }

        public string DisplayServiceDate { get; set; }

        //public decimal ServiceQuantity { get; set; }
        //public decimal ServiceRate { get; set; }
        //public decimal DiscountAmount { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public string ApprovedStatus { get; set; }

        //New for Report-------------
        public string HMCompanyProfile { get; set; }
        public string HMCompanyAddress { get; set; }
        public string HMCompanyWeb { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime ArriveDate { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public decimal RoomRate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int TotalDay { get; set; }
        public DateTime ServiceDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ServiceType { get; set; }
        public string GuestService { get; set; }
        public decimal TotalRoomCharge { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ServiceRate { get; set; }
        public decimal ServiceQuantity { get; set; }
        public decimal VatAmount { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal CitySDCharge { get; set; }
        public decimal AdditionalCharge { get; set; }
        public decimal AdvancePayment { get; set; }
        public decimal RestaurantGrandTotal { get; set; }
        public decimal ServiceTotalAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public int IsBillSplited { get; set; }
        public decimal AdvanceAmount { get; set; }
        public string Reference { get; set; }
        public decimal BillAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public int InvoiceCurrencyId { get; set; }
        public int IsPaidService { get; set; }
        public string PaymentType { get; set; }
        public decimal CurrencyExchangeRate { get; set; }
        public string ServiceSummaryType { get; set; }
        public int ServiceSummaryTypeOrderBy { get; set; }
    }
}
