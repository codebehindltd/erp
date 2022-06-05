using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpDependentBO
    {
        public int DependentId { get; set; }
        public int EmpId { get; set; }
        public int BloodGroupId { get; set; }
        public string BloodGroup { get; set; }
        public string EmployeeName { get; set; }
        public string DependentName { get; set; }
        public string Relationship { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Age { get; set; }
        public string ShowDateOfBirth { get; set; }
    }
}
