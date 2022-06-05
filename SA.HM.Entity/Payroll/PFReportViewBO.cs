using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PFReportViewBO
    {
        public int EmpId { get; set; }
        public int SalaryHeadId { get; set; }
        public string DisplayName { get; set; }
        public string SalaryHead { get; set; }
        public decimal SalaryHeadAmount { get; set; }
        public string SalaryHeadNote { get; set; }
        public DateTime ProcessDate { get; set; }

        public string ShowPFEligibilityDate { get; set; }
        public decimal? MonthlyAmount { get; set; }
        public decimal? TotalBalance { get; set; }
        public string MonthName { get; set; }
    }
}
