using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class LastMonthSalaryEmployeeBenifitsBO
    {
        public decimal AfterServiceBenefit { get; set; }
        public decimal EmployeeContribution { get; set; }
        public decimal CompanyContribution { get; set; }
        public decimal LeaveBalanceAmount { get; set; }
    }
}
