using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class RestaurantBill
    {
        public int BillId { get; set; }

        public DateTime? BillDate { get; set; }

        [StringLength(50)]
        public string BillNumber { get; set; }

        public bool? IsBillSettlement { get; set; }

        public bool? IsComplementary { get; set; }

        public bool? IsNonChargeable { get; set; }

        [StringLength(100)]
        public string SourceName { get; set; }

        public int? BillPaidBySourceId { get; set; }

        public int? CostCenterId { get; set; }

        public int? PaxQuantity { get; set; }

        [StringLength(200)]
        public string CustomerName { get; set; }

        [StringLength(100)]
        public string PayMode { get; set; }

        public int? PayModeSourceId { get; set; }

        public decimal? PaySourceCurrentBalance { get; set; }

        public int? BankId { get; set; }

        [StringLength(20)]
        public string CardType { get; set; }

        [StringLength(50)]
        public string CardNumber { get; set; }

        public DateTime? ExpireDate { get; set; }

        [StringLength(256)]
        public string CardHolderName { get; set; }

        public int? RegistrationId { get; set; }

        public decimal? SalesAmount { get; set; }

        [StringLength(50)]
        public string DiscountType { get; set; }

        public int? DiscountTransactionId { get; set; }

        public decimal? DiscountAmount { get; set; }

        public decimal? CalculatedDiscountAmount { get; set; }

        public decimal? ServiceCharge { get; set; }

        public decimal? CitySDCharge { get; set; }

        public decimal? VatAmount { get; set; }

        [StringLength(15)]
        public string AdditionalChargeType { get; set; }

        public decimal? AdditionalCharge { get; set; }

        public decimal? InvoiceServiceRate { get; set; }

        public bool? IsInvoiceServiceChargeEnable { get; set; }

        public decimal? InvoiceServiceCharge { get; set; }

        public bool? IsInvoiceCitySDChargeEnable { get; set; }

        public decimal? InvoiceCitySDCharge { get; set; }

        public bool? IsInvoiceVatAmountEnable { get; set; }

        public decimal? InvoiceVatAmount { get; set; }

        public bool? IsInvoiceAdditionalChargeEnable { get; set; }

        public decimal? InvoiceAdditionalCharge { get; set; }

        public decimal? GrandTotal { get; set; }

        public decimal? RoundedAmount { get; set; }

        public decimal? RoundedGrandTotal { get; set; }

        [StringLength(50)]
        public string BillStatus { get; set; }

        public int? BillVoidBy { get; set; }

        [StringLength(300)]
        public string Remarks { get; set; }

        [StringLength(500)]
        public string Reference { get; set; }

        public int? DealId { get; set; }

        [StringLength(50)]
        public string UserType { get; set; }

        public bool? ApprovedStatus { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public bool? IsBillReSettlement { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsLocked { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [StringLength(50)]
        public string TransactionType { get; set; }

        public long? TransactionId { get; set; }

        public bool? IsBillPreviewButtonEnable { get; set; }

        public decimal? ServiceChargeConfig { get; set; }

        public decimal? CitySDChargeConfig { get; set; }

        public decimal? VatAmountConfig { get; set; }

        public decimal? AdditionalChargeConfig { get; set; }

        public decimal? CurrencyExchangeRate { get; set; }

        [StringLength(500)]
        public string PaymentRemarks { get; set; }

        public Guid GuidId { get; set; }
        
        public Guid? RegistrationGuidId { get; set; }

        public int? ReferenceBillId { get; set; }
        public decimal? ExchangeItemVatAmount { get; set; }
        public decimal? ExchangeItemTotal { get; set; }

        public DateTime? ReturnDate { get; set; }
        public string ReferenceBillNumber { get; set; }
        public bool IsSynced { get; set; }

    }
}
