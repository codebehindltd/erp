using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmployeeTaxBO
    {
        public int TaxCollectionId { get; set; }
        public int EmpId { get; set; }
        public decimal EmployeeTaxContribution { get; set; }
        public decimal CompanyTaxContribution { get; set; }
        public System.DateTime TaxDate { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
