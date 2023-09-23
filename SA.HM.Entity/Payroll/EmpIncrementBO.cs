using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpIncrementBO
    {
        public int Id { get; set; }
        public DateTime IncrementDate { get; set; }
        public string IncrementDateDisplay { get; set; }
        public string IncrementMode { get; set; }
        public decimal Amount { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string EffectiveDateDisplay { get; set; }
        public string Remarks { get; set; }
        public string ApprovedStatus { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCodeAndName { get; set; }
        public decimal BasicSalaryBeforeIncrement { get; set; }
        public decimal BasicSalary { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public string ShowIncrementDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal IncrementAmount { get; set; }
        public decimal IncrementRate { get; set; }
    }
}
