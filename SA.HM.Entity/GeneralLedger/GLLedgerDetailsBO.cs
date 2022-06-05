using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLLedgerDetailsBO
    {
        public long LedgerDetailsId { get; set; }
        public long LedgerMasterId { get; set; }
        public long NodeId { get; set; }
        public Nullable<int> BankAccountId { get; set; }
        public string ChequeNumber { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }

        public byte? LedgerMode { get; set; }
        public decimal DRAmount { get; set; }
        public decimal CRAmount { get; set; }
        public string NodeNarration { get; set; }
        public int CostCenterId { get; set; }
        public Nullable<decimal> CurrencyAmount { get; set; }
        public string NodeType { get; set; }
        public Nullable<long> ParentId { get; set; }
        public Nullable<long> ParentLedgerId { get; set; }

        public string NodeNumber { get; set; }
        public string NodeHead { get; set; }
        public string CostCenter { get; set; }

        public bool IsEdited { get; set; }
    }
}
