using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PayrollEmpLastMonthBenifitsPaymentBO
    {
        public int BenifitId { get; set; }
        public decimal EmpId { get; set; }
        public decimal AfterServiceBenefit { get; set; }
        public decimal EmployeePFContribution { get; set; }
        public decimal CompanyPFContribution { get; set; }
        public decimal LeaveBalanceDays { get; set; }
        public decimal LeaveBalanceAmount { get; set; }
        public int ProcessYear { get; set; }
        public DateTime ProcessDateFrom { get; set; }
        public DateTime ProcessDateTo { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

    }
}
