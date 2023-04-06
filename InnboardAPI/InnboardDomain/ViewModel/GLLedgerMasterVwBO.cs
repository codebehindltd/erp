using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.ViewModel
{
    public class GLLedgerMasterVwBO
    {
        public long LedgerMasterId { get; set; }
        public int UserInfoId { get; set; }
        public int CompanyId { get; set; }
        public int ProjectId { get; set; }
        public Nullable<int> DonorId { get; set; }
        public string VoucherType { get; set; }
        public Nullable<bool> IsBankExist { get; set; }
        public string VoucherNo { get; set; }
        public Nullable<System.DateTime> VoucherDate { get; set; }
        public int CurrencyId { get; set; }
        public Nullable<decimal> ConvertionRate { get; set; }
        public string Narration { get; set; }
        public string PayerOrPayee { get; set; }
        public string ReferenceNumber { get; set; }
        public string GLStatus { get; set; }
        public Nullable<int> CheckedBy { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string ReferenceVoucherNumber { get; set; }
        public bool IsSynced { get; set; }
        //public List<GLLedgerDetailsBO> GLLedgerDetails { get; set; }
        //public List<GLVoucherApprovedInfoBO> GLVoucherApprovedInfos { get; set; }
        public string ChequeNumber { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }

        public string NodeHead { get; set; }
        public bool NodeMode { get; set; }
        public string NodeNarration { get; set; }
        public decimal Amount { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public int LedgerMode { get; set; }
        public string CheckedByName { get; set; }
        public string ApprovedByName { get; set; }
        public string VoucherDateDisplay { get; set; }
        public string CreatedDateDisplay { get; set; }
        public string CreatedByName { get; set; }		
        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
        public string VoucherTypeName { get; set; }
        public string DonorName { get; set; }

        public string CurrencyName { get; set; }
        public int CanEditDeleteAfterApproved { get; set; }
        public string VoucherDateString { get; set; }
        public bool IsCanEdit { get; set; }
        public bool IsCanDelete { get; set; }
        public bool IsCanCheck { get; set; }
        public bool IsCanApprove { get; set; }
        public decimal VoucherTotalAmount { get; set; }
        public bool IsModulesTransaction { get; set; }
    }
}
