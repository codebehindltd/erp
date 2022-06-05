using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class KotWiseVatNSChargeNDiscountNComplementaryBO
    {
        public Nullable<int> KotId { get; set; }
        public int KotDetailId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public decimal ItemTotalAmount { get; set; }
        public Nullable<decimal> InvoiceTotalAmount { get; set; }
        public Nullable<decimal> ItemUnit { get; set; }
        public Nullable<bool> IsInvoiceVatAmountEnable { get; set; }
        public Nullable<bool> IsInvoiceServiceChargeEnable { get; set; }
        public Nullable<bool> IsInvoiceCitySDChargeEnable { get; set; }
        public Nullable<bool> IsInvoiceAdditionalChargeEnable { get; set; }
        public decimal ServiceChargeConfig { get; set; }
        public decimal SDChargeConfig { get; set; }
        public decimal VatAmountConfig { get; set; }
        public decimal AdditionalChargeConfig { get; set; }
        public string BillWiseDiscountType { get; set; }
        public Nullable<decimal> BillWiseDiscountAmount { get; set; }
        public string ItemWiseDiscountType { get; set; }
        public decimal ItemWiseDiscountAmount { get; set; }
        public Nullable<int> ClassificationId { get; set; }
        public Nullable<decimal> ClassificationWiseDiscountAmount { get; set; }
        public Nullable<bool> IsComplementary { get; set; }
        public Nullable<bool> IsNonChargeable { get; set; }
        public Nullable<decimal> ItemWiseDiscount { get; set; }
        public Nullable<decimal> ClassificationWiseDiscount { get; set; }
        public Nullable<decimal> BillWiseDiscount { get; set; }
        public Nullable<decimal> FixedDiscountRatio { get; set; }
        public string ActualDiscountType { get; set; }
        public Nullable<decimal> ActualDiscount { get; set; }
        public Nullable<decimal> ActualDiscountAmount { get; set; }
        public Nullable<decimal> AmountAfterDiscount { get; set; }
        public Nullable<decimal> AmountForDistribution { get; set; }
        public Nullable<long> TransactionId { get; set; }
        public Nullable<decimal> RackRate { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<decimal> ServiceCharge { get; set; }
        public Nullable<decimal> SDCityCharge { get; set; }
        public Nullable<decimal> VatAmount { get; set; }
        public Nullable<decimal> AdditionalCharge { get; set; }
        public Nullable<decimal> CalculatedAmount { get; set; }
    }
}
