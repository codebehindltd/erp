using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
   public class EmployeeYearlyLeaveBO
    {
        public int YearlyLeaveId { get; set; }
        public int EmpId { get; set; }
        public int LeaveTypeId { get; set; }
        public int LeaveQuantity { get; set; }
        public string LeaveType { get; set; }
    }
}
