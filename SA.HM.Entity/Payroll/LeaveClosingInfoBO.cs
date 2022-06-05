using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class LeaveClosingInfoBO
    {
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<int> DesignationId { get; set; }
        public string DesignationName { get; set; }
        public Nullable<int> LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
        public decimal OpeningLeave { get; set; }
        public Nullable<int> TotalLeaveTaken { get; set; }
        public Nullable<decimal> RemainingLeave { get; set; }
        public Nullable<bool> CanCarryForward { get; set; }
        public byte MaxDayCanCarryForwardYearly { get; set; }
        public byte MaxDayCanKeepAsCarryForwardLeave { get; set; }
        public byte LastYearCarryForwardedLeave { get; set; }
        public Nullable<decimal> CurrentYearCarryForwardedLeave { get; set; }
        public Nullable<decimal> CarryForwardedLeave { get; set; }
        public Nullable<bool> CanEncash { get; set; }
        public byte MaxDayCanEncash { get; set; }
        public Nullable<decimal> CurrentYearEncashableLeave { get; set; }
    }
}
