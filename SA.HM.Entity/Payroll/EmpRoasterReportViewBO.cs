using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpRoasterReportViewBO
    {
        public string RosterName { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public DateTime RosterDate { get; set; }
        public string RosterDateDisplay { get; set; }
        public string RosterDateDayName { get; set; }
        public string TimeSlabHead { get; set; }
    }
}
