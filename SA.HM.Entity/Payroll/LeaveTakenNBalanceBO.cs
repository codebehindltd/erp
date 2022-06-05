using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class LeaveTakenNBalanceBO
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeFullName { get; set; }
        public int LeaveTypeID { get; set; }
        public string LeaveTypeName { get; set; }

        public int? TotalLeave { get; set; }
        public int? TotalTakenLeave { get; set; }
        public int? RemainingLeave { get; set; }
    }
}
