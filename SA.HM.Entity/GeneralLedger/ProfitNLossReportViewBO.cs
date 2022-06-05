using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class ProfitNLossReportViewBO
    {
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        
        public int? GroupId { get; set; }
        public string HeadDescription { get; set; }
        public int? NodeId { get; set; }
        public int? Lvl { get; set; }
        public string Notes { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountToDisplay { get; set; }
        public string Particulars { get; set; }

        public Nullable<decimal> AmountCurrentYear { get; set; }
        public Nullable<decimal> AmountToDisplayCurrentYear { get; set; }
        public Nullable<decimal> AmountPreviousYear { get; set; }
        public Nullable<decimal> AmountToDisplayPreviousYear { get; set; }

        public Nullable<System.DateTime> CurrentYearDateFrom { get; set; }
        public Nullable<System.DateTime> CurrentYearDateTo { get; set; }
        public Nullable<System.DateTime> PreviousYearDateFrom { get; set; }
        public Nullable<System.DateTime> PreviousYearDateTo { get; set; }
    }
}



