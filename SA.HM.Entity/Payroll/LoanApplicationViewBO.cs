using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class LoanApplicationViewBO
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public decimal BasicAmount { get; set; }
        public DateTime? JoinDate { get; set; }
        public int ServiceYear { get; set; }
        public DateTime? PFEligibilityDate { get; set; }
        public string DepartmentName { get; set; }
        public string Designation { get; set; }
        public int IsLoanTakenBefore { get; set; }
        public int IsThisLoanClear { get; set; }
        public string BeforeLoanTakenStatus { get; set; }
        public string LoanStatus { get; set; }
    }
}
