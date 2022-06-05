using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpAllowanceDeductionBO
    {
        public int EmpAllowDeductId { get; set; }
        public string AllowDeductType { get; set; }
        public int? DepartmentId { get; set; }
        public int? EmpId { get; set; }
        public string EmpCode { get; set; }
        public int SalaryHeadId { get; set; }
        public string SalaryHead { get; set; }
        public string AmountType { get; set; }
        public string DependsOn { get; set; }
        public decimal AllowDeductAmount { get; set; }
        public DateTime EffectFrom { get; set; }
        public DateTime EffectTo { get; set; }
        public int EffectiveYear { get; set; }
        public string Remarks { get; set; }
        public string SalaryType { get; set; }

        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public bool CanEditDelete { get; set; }
    }
}
