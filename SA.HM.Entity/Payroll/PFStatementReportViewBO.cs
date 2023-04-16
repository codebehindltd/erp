using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PFStatementReportViewBO
    {
        public long PFCollectionId { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string ShowJoinDate { get; set; }
        public string DisplayName { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string ProcessDate { get; set; }
        public decimal? EmpContribution { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? CompanyContribution { get; set; }
        public decimal? CommulativePFAmountCurrentYear { get; set; }
        public decimal? PreviousYearBalance { get; set; }
        public decimal? CurrentYearBalance { get; set; }
        public decimal? Interest { get; set; }
        public string ShowPFEligibilityDate { get; set; }
        public decimal? TotalContribution { get; set; }
        public string LastDeductedMonthYear { get; set; }
        public string CurrencyName { get; set; }
        public byte[] QrEmployeeImage { get; set; }
    }
}
