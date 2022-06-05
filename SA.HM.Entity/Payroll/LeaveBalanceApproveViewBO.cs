using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class LeaveBalanceApproveViewBO : LeaveTypeBO
    {
        public int EmpId { get; set; }
        public string Employeename { get; set; }
        public string EmpCode { get; set; }
        public string LeaveType { get; set; }
        public int LeaveTaken { get; set; }
        public int RemainingLeave { get; set; }
        public int CarryForwardedLeave { get; set; }
        public int TotalCarryforwardLeave { get; set; }
    }
}

//EmpId EmpCode Employeename LeaveTypeId LeaveType YearlyLeave LeaveTaken 
//RemainingLeave  CanCarryForward MaxDayCanKeepAsCarryForwardLeave    MaxDayCanCarryForwardYearly 
//CarryForwardedLeave CanEncash MaxDayCanEncash TotalCarryforwardLeave
