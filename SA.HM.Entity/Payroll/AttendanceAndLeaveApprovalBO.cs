using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class AttendanceAndLeaveApprovalBO
    {
        public Nullable<int> EmpId { get; set; }
        public Nullable<int> MonthId { get; set; }
        public Nullable<int> AttendanceId { get; set; }
        public Nullable<System.DateTime> AttendanceDate { get; set; }
        public Nullable<System.TimeSpan> InTime { get; set; }
        public Nullable<System.TimeSpan> OutTime { get; set; }
        public Nullable<int> LeaveId { get; set; }
        public Nullable<int> LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public string LeaveMode { get; set; }
        public string Reamrks { get; set; }
        public string AttendanceColor { get; set; }
        
        public string AttendanceDateStr { get; set; }
        public string InTimeStr { get; set; }
        public string OutTimeStr { get; set; }
    }
}
