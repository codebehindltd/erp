using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class LoanCollectionBO
    {
        public int CollectionId { get; set; }
        public int LoanId { get; set; }
        public int EmpId { get; set; }
        public int InstallmentNumber { get; set; }
        public DateTime CollectionDate { get; set; }
        public decimal CollectedLoanAmount { get; set; }
        public decimal CollectedInterestAmount { get; set; }
        public string ApprovedStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public decimal LoanAmount { get; set; }
        public decimal DueAmount { get; set; }
        public decimal OverdueAmount { get; set; }
    }
}
