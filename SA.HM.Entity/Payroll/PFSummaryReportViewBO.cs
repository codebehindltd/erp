using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PFSummaryReportViewBO
    {        
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public decimal? OpeningBalance { get; set; }
        public decimal? EmpContribution { get; set; }
        public decimal? EmpInterest { get; set; }
        public decimal? CompanyContribution { get; set; }
        public decimal? CompanyInterest { get; set; }
        public decimal? YearTotal { get; set; }
        public decimal? EndBalance { get; set; }
    }
}
