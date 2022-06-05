using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class PMSupplierPaymentLedgerBO
    {
        public long SupplierPaymentId { get; set; }
        public string PaymentType { get; set; }
        public int BillId { get; set; }
        public string LedgerNumber { get; set; }
        public string BillNumber { get; set; }
        public DateTime BillDate { get; set; }
        public string BillDateDisplay { get; set; }
        public decimal BillAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public int SupplierId { get; set; }
        public int CurrencyId { get; set; }
        public decimal ConvertionRate { get; set; }
        public decimal DRAmount { get; set; }
        public decimal CRAmount { get; set; }
        public decimal CurrencyAmount { get; set; }
        public long AccountsPostingHeadId { get; set; }
        public string Remarks { get; set; }
        public string ChequeNumber { get; set; }
        public string PaymentStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string SupplierName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyType { get; set; }
        public bool IsBillGenerated { get; set; }
        public decimal DueAmount { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal AdvanceAmountRemaining { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPhone { get; set; }
        
        public Int64 PaymentDetailsId { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}
