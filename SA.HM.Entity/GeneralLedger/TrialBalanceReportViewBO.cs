using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class TrialBalanceReportViewBO
    {
        public Nullable<long> NodeId { get; set; }
        public string NodeNumber { get; set; }
        public string NodeHead { get; set; }
        public string HeadType { get; set; }
        public Nullable<decimal> OpeningBalance { get; set; }
        public Nullable<decimal> DRAmount { get; set; }
        public Nullable<decimal> CRAmount { get; set; }
        public Nullable<decimal> ClosingBalance { get; set; }

        public string GroupNodeNumber { get; set; }
        public string GroupNodeHead { get; set; }

        public Nullable<int> NodeOrder { get; set; }
        public Nullable<int> NodeLevel { get; set; }
        public Nullable<int> GroupOrder { get; set; }
        public Nullable<int> GroupLevel { get; set; }
        public Nullable<int> GroupId { get; set; }

    }
}
