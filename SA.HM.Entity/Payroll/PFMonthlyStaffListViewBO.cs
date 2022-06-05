using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PFMonthlyStaffListViewBO
    {
        public int EmpId { get; set; }
        public string EmployeeName { get; set; }
        public string EmpCode { get; set; }
        public int MonthNo { get; set; }
        public string PFMonthName { get; set; }
        public decimal? OpeningBalance { get; set; }
        public decimal? EmpContribution { get; set; }
        public decimal? CompanyContribution { get; set; }
        public decimal? CommulativePFAmount { get; set; }
        public DateTime PFDate { get; set; }
    }
}
