using HotelManagement.Entity.HMCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpAttendanceReportBO : PermissionViewBO
    {
        public int EmpId { get; set; }
        public int AttendanceId { get; set; }
        public string EmpCode { get; set; }
        public string DisplayName { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public int DesignationId { get; set; }
        public string Designation { get; set; }
        public string AttendanceStatus { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public DateTime SlabStartTime { get; set; }
        public DateTime SlabEndTime { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string WorkingHour { get; set; }
        public string LateTime { get; set; }
        public string LateApplicationDateDisplay { get; set; }
        public string OTHour { get; set; }
        public int TimeSlabId { get; set; }
        public string TimeSlabHead { get; set; }
        public DateTime RosterDate { get; set; }
        public string Remarks { get; set; }
        public string CancelReason { get; set; }
        public string AttendanceDateDisplay { get; set; }
        public string Description { get; set; }

    }
}
