using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class AttendanceAndLeaveApprovalViewBO
    {
        public List<AttendanceAndLeaveApprovalBO> AttendanceAndLeaveApproval { get; set; }
        public List<LeaveTypeBO> LeaveType { get; set; }
        public List<CustomFieldBO> LeaveMode { get; set; }
    }
}
