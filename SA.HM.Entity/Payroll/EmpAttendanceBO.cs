using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpAttendanceBO
    {
        public int AttendanceId { get; set; }
        public int EmpId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string stringAttendenceDate { get; set; }
        public DateTime EntryTime { get; set; }
        public string stringEntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public string stringExitTime { get; set; }
        public string Remark { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string EmployeeName { get; set; }
        public string EmpCode { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string StartHour { get; set; }
        public string StartMin { get; set; }
        public string StartAMPM { get; set; }

        public string AttendenceStatus { get; set; }
        public string CancelReason { get; set; }
        public string CheckedByName { get; set; }
        public string ApprovedByName { get; set; }
        public int? CheckedBy { get; set; }
        public DateTime? CheckedDate { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public bool IsSynced { get; set; }

        public string EndHour { get; set; }
        public string EndMin { get; set; }
        public string EndAMPM { get; set; }
    }
}
