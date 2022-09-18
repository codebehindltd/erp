using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class BankSalaryAdviceBO
    {
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public string AccountName { get; set; }
        public Nullable<decimal> HomeTakenAmount { get; set; }
        public string AccountNumber { get; set; }
        public string RouteNumber { get; set; }
        public int? BankId { get; set; }
        public string BankName { get; set; }
        public string Designation { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string WorkStationName { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<int> DonorId { get; set; }
        public string Project { get; set; }
    }
}
