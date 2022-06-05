using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLReportDynamicallyForReportBO
    {
        public string HMCompanyProfile { get; set; }
        public string HMCompanyAddress { get; set; }
        public string HMCompanyWeb { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public long RCId { get; set; }
        public string ReportType { get; set; }
        public string AccountType { get; set; }
        public Nullable<int> Lvl { get; set; }
        public string NodeHead { get; set; }
        public string NodeNumber { get; set; }
        public string GroupName { get; set; }
        public decimal CalculatedNodeAmount { get; set; }
        public string CalculationType { get; set; }
        public Nullable<bool> IsActiveLinkUrl { get; set; }
        public string Url { get; set; }
    }
}
