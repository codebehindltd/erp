﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class LedgerBookReportBO
    {
        public string VoucherNo { get; set; }
        public Nullable<System.DateTime> VoucherDate { get; set; }
        public Nullable<long> DealId { get; set; }
        public Nullable<long> LedgerId { get; set; }
        public Nullable<long> NodeId { get; set; }
        public string NodeNumber { get; set; }
        public string NodeHead { get; set; }
        public string Narration { get; set; }
        public Nullable<decimal> OpeningBalance { get; set; }
        public Nullable<decimal> DRAmount { get; set; }
        public Nullable<decimal> CRAmount { get; set; }
        public Nullable<decimal> BalanceAmount { get; set; }
        public Nullable<decimal> NodeBalanceAmount { get; set; }
        public Nullable<decimal> ClosingBalance { get; set; }
        public Nullable<long> ParentNodeId { get; set; }
        public string ParentNodeNumber { get; set; }
        public string ParentNodeHead { get; set; }

        public Nullable<int> GroupId { get; set; }
        public string NotesNumber { get; set; }


        public Nullable<decimal> DRAmountCurrentYear { get; set; }
        public Nullable<decimal> CRAmountCurrentYear { get; set; }
        public Nullable<decimal> BalanceCurrentYear { get; set; }
        public Nullable<decimal> DRAmountPreviousYear { get; set; }
        public Nullable<decimal> CRAmountPreviousYear { get; set; }
        public Nullable<decimal> BalancePreviousYear { get; set; }
        public Nullable<System.DateTime> CurrentYearDateFrom { get; set; }
        public Nullable<System.DateTime> CurrentYearDateTo { get; set; }
        public Nullable<System.DateTime> PreviousYearDateFrom { get; set; }
        public Nullable<System.DateTime> PreviousYearDateTo { get; set; }

    }
}
