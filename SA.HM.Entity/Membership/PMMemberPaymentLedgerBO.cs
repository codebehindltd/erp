using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Membership
{
    public class PMMemberPaymentLedgerBO
    {
        public long MemberPaymentId { get; set; }
        public string PaymentType { get; set; }
        public string LedgerNumber { get; set; }
        public string ModuleName { get; set; }
        public long BillId { get; set; }
        public string BillNumber { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int MemberId { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public decimal? ConvertionRate { get; set; }
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
        public string MemberName { get; set; }
        public string FullName { get; set; }
        public string MemberAddress { get; set; }
        public string EmailAddress { get; set; }
        public string WebAddress { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string MobileNumber { get; set; }
        public string TelephoneNumber { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PaidAmountCurrent { get; set; }
        public decimal? DueAmount { get; set; }
        public decimal AdvanceAmount { get; set; }
        public bool IsBillGenerated { get; set; }
        public bool IsBillAdjusted { get; set; }
        public decimal UsdConversionRate { get; set; }
        public decimal UsdBillAmount { get; set; }
        public long BillGenerationId { get; set; }
        public long RefMemberPaymentId { get; set; }
    }
}
