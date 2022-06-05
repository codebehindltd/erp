using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLLedgerMasterBO
    {
        public long LedgerMasterId { get; set; }
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
        public List<GLLedgerDetailsBO> GLLedgerDetails { get; set; }
        public List<GLVoucherApprovedInfoBO> GLVoucherApprovedInfos { get; set; }
        public string ChequeNumber { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }
    }
}
