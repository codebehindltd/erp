using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmployeePayslipBO
    {
        public Nullable<int> ProcessId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string Location { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<System.DateTime> SalaryDateFrom { get; set; }
        public Nullable<System.DateTime> SalaryDateTo { get; set; }
        public Nullable<int> PaidDays { get; set; }
        public Nullable<int> PayrollCurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public Nullable<decimal> ConvertionRate { get; set; }
        public Nullable<decimal> BasicAmountInEmployeeCurrency { get; set; }
        public string SalaryEffectiveness { get; set; }
        public Nullable<decimal> BasicAmount { get; set; }
        public Nullable<int> SalaryHeadId { get; set; }
        public string SalaryHead { get; set; }
        public string SalaryType { get; set; }
        public Nullable<decimal> SalaryAmount { get; set; }
        public Nullable<decimal> GrossAmount { get; set; }
        public Nullable<decimal> TotalAllowance { get; set; }
        public Nullable<decimal> TotalDeduction { get; set; }
        public Nullable<decimal> HomeTakenAmount { get; set; }
        public string ShowJoiningDate { get; set; }        
        public Nullable<System.DateTime> ContractEndDate { get; set; }
        public string ShowContractEndDate { get; set; }
        public string EmpType { get; set; }
        public Nullable<int> SalaryTypeRank { get; set; }

        //public int ProcessId { get; set; }
        //public Nullable<int> EmpId { get; set; }
        //public string EmpCode { get; set; }
        //public string EmployeeName { get; set; }
        public Nullable<System.DateTime> JoinDate { get; set; }
        //public Nullable<System.DateTime> ContractEndDate { get; set; }        
        public Nullable<int> EmpTypeId { get; set; }
        //public string EmpType { get; set; }
        //public string Designation { get; set; }
        //public Nullable<int> LocationId { get; set; }
        public string WorkStation { get; set; }
        //public Nullable<int> DepartmentId { get; set; }
        //public string DepartmentName { get; set; }
        //public Nullable<System.DateTime> SalaryDateFrom { get; set; }
        //public Nullable<System.DateTime> SalaryDateTo { get; set; }
        //public Nullable<int> PaidDays { get; set; }
        //public Nullable<int> DonorId { get; set; }
        //public string Project { get; set; }
        //public Nullable<decimal> BasicAmount { get; set; }
        //public Nullable<int> SalaryHeadId { get; set; }
        //public string SalaryHead { get; set; }
        public string SalaryHeadNote { get; set; }
        //public string SalaryType { get; set; }
        //public string SalaryEffectiveness { get; set; }
        //public Nullable<decimal> SalaryAmount { get; set; }
        //public Nullable<decimal> GrossAmount { get; set; }
        //public Nullable<decimal> HomeTakenAmount { get; set; }
        public int EmployeeRank { get; set; }
        //public int SalaryTypeRank { get; set; }

        //public Nullable<decimal> TotalAllowance { get; set; }
        //public Nullable<decimal> TotalDeduction { get; set; }
        public decimal? MedicalAllowance { get; set; }
        public decimal? NSSFEmployeeContribution { get; set; }
        public decimal? NSSFCompanyContribution { get; set; }
        public string CompanyContributionLabelTitle { get; set; }
        public decimal? CurrencyExchangeRate { get; set; }
        public int? BankId { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }

        //public int PayrollCurrencyId { get; set; }
        //public string CurrencyName { get; set; }
        //public decimal ConvertionRate { get; set; }
        //public decimal BasicAmountInEmployeeCurrency { get; set; }
        public byte[] QrEmployeeImage { get; set; }
        
    }
}
