using HotelManagement.Entity.HMCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestExtraServiceBillApprovedBO:BaseEntity
    {
        public int ApprovedId { get; set; }
        public int? CostCenterId { get; set; }
        public int? RegistrationId { get; set; }
        public string RoomNumber { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int ServiceBillId { get; set; }
        public DateTime? ServiceDate { get; set; }
        public string ServiceType { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public decimal? ServiceQuantity { get; set; }
        public decimal? ServiceRate { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? VatAmount { get; set; }
        public decimal? ServiceCharge { get; set; }
        public decimal? InvoiceServiceRate { get; set; }
        public bool? IsServiceChargeEnable { get; set; }
        public decimal? InvoiceServiceCharge { get; set; }
        public bool? IsVatAmountEnable { get; set; }
        public decimal? InvoiceVatAmount { get; set; }
        public string ApprovedStatus { get; set; }
        public string PaymentMode { get; set; }
        public bool? IsPaidService { get; set; }
        public bool? IsPaidServiceAchieved { get; set; }
        public bool? IsDayClosed { get; set; }

        public decimal? CalculatedTotalAmount { get; set; }
        public bool? IsAdditionalChargeEnable { get; set; }
        public decimal? InvoiceAdditionalCharge { get; set; }
        public decimal? CurrencyExchangeRate { get; set; }
        public decimal? InvoiceUsdRackRate { get; set; }
        public decimal? InvoiceUsdServiceCharge { get; set; }
        public decimal? InvoiceUsdVatAmount { get; set; }
        public decimal? CitySDCharge { get; set; }
        public decimal? AdditionalCharge { get; set; }
        public bool? IsCitySDChargeEnable { get; set; }
        public decimal? InvoiceCitySDCharge { get; set; }
        public Guid? GuidId { get; set; }

    }
}
