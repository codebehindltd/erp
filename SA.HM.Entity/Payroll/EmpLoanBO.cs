using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpLoanBO
    {
        public int LoanId { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public string LoanNumber { get; set; }
        public string LoanType { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal InterestRate { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal DueAmount { get; set; }
        public decimal DueInterestAmount { get; set; }
        public decimal OverDueAmount { get; set; }
        public int LoanTakenForPeriod { get; set; }
        public string LoanTakenForMonthOrYear { get; set; }
        public int InstallmentLength { get; set; }
        public decimal PerInstallLoanAmount { get; set; }
        public decimal PerInstallInterestAmount { get; set; }
        public System.DateTime LoanDate { get; set; }
        public string LoanStatus { get; set; }
        public string ApprovedStatus { get; set; }
        public int? CheckedBy { get; set; }
        public int? ApprovedBy { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public int IsAutoLoanCollectionProcessEnable { get; set; }
        public bool Status { get; set; }
        public bool IsCanEdit { get; set; }
        public bool IsCanDelete { get; set; }
        public bool IsCanChecked { get; set; }
        public bool IsCanApproved { get; set; }
        public int InstallmentNumber { get; set; }
    }
}
