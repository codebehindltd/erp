using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class OvertimeHourOfEmployeeeBO
    {
        public int EmpId { get; set; }
        public System.DateTime AttendanceDate { get; set; }
        public Nullable<System.DateTime> EntryTime { get; set; }
        public Nullable<System.DateTime> ExitTime { get; set; }
        public Nullable<int> TotalHour { get; set; }
        public Nullable<int> OTHour { get; set; }

        public string EmployeeName { get; set; }
        public string EmpCode { get; set; }
        public string Designation { get; set; }
    }
}
