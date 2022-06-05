using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpPayslipReportViewBO
    {
        //ProcessId	EmpId	EmpCode	EmployeeName	JoinDate	EmpTypeId	EmpType	
        //Designation	LocationId	WorkStation	DepartmentId	DepartmentName	SalaryDateFrom	
        //SalaryDateTo	PaidDays	DonorId	Project	BasicAmount	SalaryHeadId	SalaryHead	
        //SalaryType	SalaryAmount	GrossAmount	HomeTakenAmount

        public int ProcessId { get; set; }
        public string ProcessDate { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public string DisplayName { get; set; }
        public string JoinDate { get; set; }
        public Nullable<int> EmpTypeId { get; set; }
        public string EmpType { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string WorkStation { get; set; }
        public string TransactionType { get; set; }
        public string SalaryType { get; set; }
        public Nullable<int> SalaryHeadId { get; set; }
        public string SalaryHeadNote { get; set; }
        public string SalaryCategory { get; set; }
        public string SalaryHead { get; set; }
        public Nullable<decimal> SalaryHeadAmount { get; set; }
        public Nullable<bool> IsBonusPaid { get; set; }
        public Nullable<decimal> BasicSalary { get; set; }
        public Nullable<decimal> GrossSalary { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public Nullable<int> PaidDays { get; set; }
        public string Project { get; set; }
        //public Nullable<System.DateTime> ResignationDate { get; set; }
        public string ResignationDate { get; set; }
        
       

    }
}
