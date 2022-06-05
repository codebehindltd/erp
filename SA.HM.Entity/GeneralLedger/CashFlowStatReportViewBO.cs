using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class CashFlowStatReportViewBO
    {
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }      
        public string CashFlowHead { get; set; }
        public decimal? Balance { get; set; }
        public decimal? DRAmount { get; set; }
        public decimal? CRAmount { get; set; }
        public string URL { get; set; }

        public Nullable<int> GroupId { get; set; }
        public string GroupName { get; set; }
        public Nullable<int> ParentNodeId { get; set; }
        public string ParentNodeHead { get; set; }
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



