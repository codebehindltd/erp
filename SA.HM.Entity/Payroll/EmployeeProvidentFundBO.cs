using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmployeeProvidentFundBO
    {
        public int PFCollectionId { get; set; }
        public int EmpId { get; set; }
        public decimal EmployeeContribution { get; set; }
        public decimal CompanyContribution { get; set; }
        public Nullable<decimal> ProvidentFundInterest { get; set; }
        public System.DateTime PFDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
