using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLJournalRegisterInfoForReportBO
    {
        public Nullable<int> DealId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public Nullable<byte> VoucherMode { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherDate { get; set; }
        public string Narration { get; set; }
        public int LedgerId { get; set; }
        public decimal LedgerAmount { get; set; }
        public string NodeNarration { get; set; }
        public long NodeId { get; set; }
        public byte LedgerMode { get; set; }
        public string NodeHead { get; set; }
        public string NodeNumber { get; set; }
        public string ChequeNumber { get; set; }
        public Nullable<bool> NodeMode { get; set; }
        public decimal Amount { get; set; }
        public Nullable<decimal> DebitAmount { get; set; }
        public Nullable<decimal> CreditAmount { get; set; }
        public string InWordAmount { get; set; }
        public Nullable<byte> CashChequeMode { get; set; }
        public string VcheqNo { get; set; }
    }
}
