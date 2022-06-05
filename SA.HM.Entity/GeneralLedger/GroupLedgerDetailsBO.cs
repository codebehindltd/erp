using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GroupLedgerDetailsBO
    {        
        public Nullable<long> LedgerMasterId { get; set; }
        public Nullable<System.DateTime> VoucherDate { get; set; }
        public string VoucherDateDisplay { get; set; }
        public string VoucherType { get; set; }
        public string VoucherNo { get; set; }
        public Nullable<long> NodeId { get; set; }
        public string NodeNumber { get; set; }
        public string NodeHead { get; set; }
        public Nullable<decimal> DRAmount { get; set; }
        public Nullable<decimal> CRAmount { get; set; }
        public Nullable<decimal> TransactionalBalance { get; set; }
        public Nullable<decimal> ClosingBalance { get; set; }
        public Nullable<decimal> CommulativeBalance { get; set; }
        public string Narration { get; set; }
        public string NodeNarration { get; set; }
        public string NodeType { get; set; }
        public Nullable<bool> IsTransactionalHead { get; set; }
        public Nullable<long> GroupNodeId { get; set; }
        public string GroupNodeNumber { get; set; }
        public string GroupNodeHead { get; set; }
        public Nullable<bool> IsTransactionalHeadGroup { get; set; }
        public Nullable<int> Rnk { get; set; }

        public Nullable<long> LedgerNodeId  { get; set; }
        public string LedgerNodeNumber  { get; set; }
        public string LedgerNodeHead { get; set; }
        public string NodeAcccount { get; set; }
    }
}
