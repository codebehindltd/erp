using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpLoanCollectionDetails
    {
        public int EmpId { get; set; }
        public int LoanId { get; set; }
        public int CollectionId { get; set; }
        public string LoanType { get; set; }
        public int LoanTakenForPeriod { get; set; }
        public string LoanTakenForMonthOrYear { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal DueAmount { get; set; }
        public Nullable<System.DateTime> CollectionDate { get; set; }
        public Nullable<int> InstallmentNumber { get; set; }
        public Nullable<decimal> CollectionAmount { get; set; }

        public string ApprovedStatus { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
    }
}
