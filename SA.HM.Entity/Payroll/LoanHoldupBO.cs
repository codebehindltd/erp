using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class LoanHoldupBO
    {
        public int LoanHoldupId { get; set; }
        public int LoanId { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public string ReasonForLoanHoldup { get; set; }
        public DateTime LoanHoldupDateFrom { get; set; }
        public DateTime LoanHoldupDateTo { get; set; }
        public int DurationForLoanHoldup { get; set; }
        public string HoldForMonthOrYear { get; set; }
        public int InstallmentNumberWhenLoanHoldup { get; set; }
        public decimal DueAmount { get; set; }
        public decimal OverDueAmount { get; set; }
        public int ApprovedBy { get; set; }
        public string ApproveEmpCode { get; set; }
        public string ApprovedEmployeeName { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public string Remarks { get; set; }
        public string HoldupStatus { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
