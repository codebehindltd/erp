using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpExperienceBO
    {
        public int ExperienceId { get; set; }
        public int EmpId { get; set; }
        public string EmployeeName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyUrl { get; set; }
        public DateTime JoinDate { get; set; }
        public string JoinDepartment { get; set; }
        public string JoinDesignation { get; set; }
        public DateTime? LeaveDate { get; set; }
        public string LeaveDepartment { get; set; }
        public string LeaveDesignation { get; set; }
        public string LeaveCompany { get; set; }
        public string Achievements { get; set; }
        public string ShowJoinDate { get; set; }
        public string ShowLeaveDate { get; set; }
    }
}
