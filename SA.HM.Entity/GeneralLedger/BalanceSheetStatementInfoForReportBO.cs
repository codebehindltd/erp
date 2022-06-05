using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class BalanceSheetStatementInfoForReportBO
    {
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public Nullable<int> ParentGroupId { get; set; }
        public string ParentGroup { get; set; }
        public Nullable<int> GroupId { get; set; }
        public string Particulars { get; set; }
        public Nullable<Int64> NodeId { get; set; }
        public string Notes { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> Lvl { get; set; }
        public string NotesNumber { get; set; }
        public string URL { get; set; }

        public Nullable<decimal> AmountCurrentYear { get; set; }
        public Nullable<decimal> AmountPreviousYear { get; set; }
        public Nullable<System.DateTime> CurrentYearDateFrom { get; set; }
        public Nullable<System.DateTime> CurrentYearDateTo { get; set; }
        public Nullable<System.DateTime> PreviousYearDateFrom { get; set; }
        public Nullable<System.DateTime> PreviousYearDateTo { get; set; }
    }
}
