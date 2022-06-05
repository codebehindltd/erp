using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class LeaveBalanceClosingViewBO
    {
        public long EmpId { get; set; }
        public long LeaveClosingId { get; set; }
        public string EmployeeName { get; set; }
        public string EmpCode { get; set; }
        public string Deparment { get; set; }
        public string Designation { get; set; }
    }
}
