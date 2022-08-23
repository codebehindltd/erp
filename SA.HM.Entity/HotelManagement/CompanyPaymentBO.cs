using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class CompanyPaymentBO
    {
        public Int64 PaymentId { get; set; }
        public Int64 CompanyBillId { get; set; }
        public string PaymentFor { get; set; }
        public string AdjustmentType { get; set; }
        public Int64 CompanyPaymentAdvanceId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string LedgerNumber { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public int CompanyId { get; set; }
        public string CurrencyName { get; set; }
        public string Remarks { get; set; }
        public string PaymentType { get; set; }
        public int AccountingPostingHeadId { get; set; }
        public string CompanyName { get; set; }
        public string ChequeNumber { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? ConvertionRate { get; set; }
        public string ApprovedStatus { get; set; }
        public string CurrencyType { get; set; }
        public string BillNumber { get; set; }
        public string CompanyAddress { get; set; }
        public string PaymentDisplayDate { get; set; }
        public decimal? PaymentAmount { get; set; }

        public int? AdjustmentAccountHeadId { get; set; }
        public decimal? PaymentAdjustmentAmount { get; set; }
        public DateTime? ChequeDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public int? CheckedBy { get; set; }
        public int? ApprovedBy { get; set; }
        public bool IsCanEdit { get; set; }
        public bool IsCanDelete { get; set; }
        public bool IsCanChecked { get; set; }
        public bool IsCanApproved { get; set; }

    }
}
