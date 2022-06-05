using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class CostCenterWiseReconcilationSummaryViewBO
    {
        public int SalaryHeadId { get; set; }
        public string SalaryHead { get; set; }
        public decimal SalaryAmount { get; set; }
        public decimal TotoalAllowance { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal NetSalary { get; set; }
        public int TotalEmployee { get; set; }

        public decimal SalaryAmountYtd { get; set; }
        public decimal TotoalAllowanceYtd { get; set; }
        public decimal TotalDeductionYtd { get; set; }
        public decimal NetSalaryYtd { get; set; }
    }
}
