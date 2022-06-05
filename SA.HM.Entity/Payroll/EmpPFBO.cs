using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpPFBO
    {
        public long PFCollectionId { get; set; }
        public int EmpId { get; set; }
        public decimal EmployeeContribution { get; set; }
        public decimal CompanyContribution { get; set; }
        public decimal? ProvidentFundInterest { get; set; }
        public decimal CommulativeEmpContribution { get; set; }
        public decimal CommulativeCompanyContribution { get; set; }
        public decimal CommulativeInterestAmount { get; set; }
        public decimal CommulativePFAmount { get; set; }
        public DateTime PFDateFrom { get; set; }
        public DateTime PFDateTo { get; set; }
        public Int16 PFYear { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
