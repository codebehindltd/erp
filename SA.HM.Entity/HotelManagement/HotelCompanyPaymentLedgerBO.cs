using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class HotelCompanyPaymentLedgerBO
    {
        public long CompanyPaymentId { get; set; }
        public string PaymentType { get; set; }
        public string LedgerNumber { get; set; }
        public string ModuleName { get; set; }
        public int BillId { get; set; }
        public string BillNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentDateDisplay { get; set; }
        public long CompanyBillId { get; set; }
        public string BillDetails { get; set; }
        public int CompanyId { get; set; }
        public int CurrencyId { get; set; }
        public decimal ConvertionRate { get; set; }
        public decimal DRAmount { get; set; }
        public decimal CRAmount { get; set; }
        public decimal CurrencyAmount { get; set; }
        public string Remarks { get; set; }
        public string PaymentStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public long AccountsPostingHeadId { get; set; }
        public string CurrencyType { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string EmailAddress { get; set; }
        public string WebAddress { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string TelephoneNumber { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PaidAmountCurrent { get; set; }
        public decimal DueAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal UsdConversionRate { get; set; }
        public decimal UsdBillAmount { get; set; }
        public bool IsBillGenerated { get; set; }
        public bool IsBillAdjusted { get; set; }
        public decimal AdvanceAmountRemaining { get; set; }
        public long BillGenerationId { get; set; }
        public long RefCompanyPaymentId { get; set; }
        public List<CompanyPaymentLedgerVwBo> CompanyLedger { get; set; }
        public int PaymentId { get; set; }
        public decimal BillAmount { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public string AdjustmentDetails { get; set; }
        public decimal CollectionAmount { get; set; }
        public string InvoiceDetails { get; set; }
        public string TransactionBy { get; set; }
        public int ColumnOrderDisplay { get; set; }
        public string ColumnAgingTitle { get; set; }
        public decimal ColumnAgingBalance { get; set; }
        public int TransactionAge { get; set; }
    }
}
