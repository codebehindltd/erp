using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class SalarySheetBO
    {
        public int ProcessId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public string Position { get; set; }
        public int? LocationId { get; set; }
        public string Location { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DaysFrom { get; set; }
        public string DaysTo { get; set; }
        public Nullable<int> PaidDays { get; set; }
        public string DonorName { get; set; }
        public string Company { get; set; }
        public string Project { get; set; }
        public Nullable<decimal> GrossSalary { get; set; }
        public Nullable<decimal> TaxableIncome { get; set; }
        public Nullable<decimal> NetSalary { get; set; }
        public Nullable<int> PayHeadId { get; set; }
        public string PayHead { get; set; }
        public string PayHeadType { get; set; }
        public string PayHeadDescription { get; set; }
        public Nullable<decimal> PayHeadAmount { get; set; }
        public string Designation { get; set; }
        public DateTime? SalaryDateFrom { get; set; }
        public DateTime? SalaryDateTo { get; set; }
        public decimal BasicAmount { get; set; }
        public decimal MedicalAllowance { get; set; }
        public decimal NSSFEmployeeContribution { get; set; }
        public int SalaryHeadId { get; set; }
        public string SalaryHead { get; set; }
        public string SalaryType { get; set; }
        public decimal SalaryAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal TotalAllowance { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal HomeTakenAmount { get; set; }
        public int SalaryTypeRank { get; set; }
        public string SalaryEffectiveness { get; set; }
        public string SalaryMonth { get; set; }
        public string SalaryYearMonth { get; set; }
        public string ShowJoiningDate { get; set; }
        public int PayrollCurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public decimal ConvertionRate { get; set; }
        public decimal BasicAmountInEmployeeCurrency { get; set; }
        public Int64 Serial { get; set; }
    }
}
