using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpBankInfoBO
    {
        public int BankInfoId { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public int? BankId { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string BankRemarks { get; set; }
        public decimal NetSalary { get; set; }
        public string Remarks { get; set; }
    }
}
