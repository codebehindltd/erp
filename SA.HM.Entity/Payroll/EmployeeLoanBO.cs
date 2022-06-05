using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmployeeLoanBO
    {
        public int LoanId { get; set; }
        public int EmpId { get; set; }
        public string LoanType { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal InterestRate { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal DueAmount { get; set; }
        public decimal DueInterestAmount { get; set; }
        public int LoanTakenForPeriod { get; set; }
        public string LoanTakenForMonthOrYear { get; set; }
        public int InstallmentLength { get; set; }
        public decimal PerInstallAmount { get; set; }
        public string ApprovedBy { get; set; }
        public System.DateTime LoanApprovedDate { get; set; }
        public System.DateTime LoanDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
