using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class SalaryFormulaBO
    {
        public int FormulaId { get; set; }
        public string TransactionType { get; set; }
        public int GradeIdOrEmployeeId { get; set; }
        public string Grade { get; set; }
        public string EmplyeeName { get; set; }
        public string EmployeeCode { get; set; }
        public int SalaryHeadId { get; set; }
        public string SalaryHead { get; set; }
        public string SalaryType { get; set; }
        public int? DependsOn { get; set; }
        public string DependsOnHead { get; set; }
        public decimal Amount { get; set; }
        public string AmountType { get; set; }
        public bool ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public string ContributionType { get; set; }
    }

    public class SalaryFormulaViewBO
    {
        public List<SalaryFormulaBO> EmployeeIndividualWise { get; set; }
        public List<SalaryFormulaBO> EmployeeGradeWise { get; set; }
    }
}
