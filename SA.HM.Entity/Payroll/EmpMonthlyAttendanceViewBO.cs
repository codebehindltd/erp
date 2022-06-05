using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpMonthlyAttendanceViewBO
    {
        public int EmpId { get; set; }
        public Int64 SL { get; set; }
        public string DisplayName { get; set; }
        public string MonthName { get; set; }
        public int NoOfDays { get; set; }

        public int AttendanceDay { get; set; }
        public string EmpCode { get; set; }        
        public string Department { get; set; }
        public string Designation { get; set; }

        public string Entrytime { get; set; }
        public string Exittime { get; set; }
        public int Day { get; set; }
        public int WorkingDaysThisMonth { get; set; }
        public int TotalPresentThisMonth { get; set; }
        public int TotalLeaveThisMonth { get; set; }
        public int TotalHolidayThisMonth { get; set; }
        public int TotalDayOffThisMonth { get; set; }
        public int TotalAbsentThisMonth { get; set; }
        public int TotalDaysThisMOnth { get; set; }

    }
}
