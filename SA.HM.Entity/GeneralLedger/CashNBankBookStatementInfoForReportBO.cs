using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class CashNBankBookStatementInfoForReportBO
    {
        public string NameOfAccount { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public long CBCode { get; set; }
        public Nullable<long> NodeId { get; set; }
        public Nullable<long> AncestorId { get; set; }
        public string NodeNumber { get; set; }
        public string NodeHead { get; set; }
        public int Lvl { get; set; }
        public string Hierarchy { get; set; }
        public string HierarchyIndex { get; set; }
        public Nullable<byte> NodeMode { get; set; }
        public Nullable<int> CostCentreId { get; set; }
        public Nullable<int> DealId { get; set; }
        public Nullable<int> LedgerId { get; set; }
        public string VoucherType { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherDate { get; set; }
        public string CounterPart { get; set; }
        public string ChequeNumber { get; set; }
        public Nullable<byte> LedgerMode { get; set; }
        public Nullable<decimal> PriorBalance { get; set; }
        public Nullable<decimal> ReceivedAmount { get; set; }
        public Nullable<decimal> PaidAmount { get; set; }
        public string NodeNarration { get; set; }
        public Nullable<decimal> Balance { get; set; }
    }
}
