using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpOverTimeBO
    {
        public Int64 OverTimeId { get; set; }
        public int EmpId { get; set; }
        public DateTime OverTimeDate { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }
        public int TotalHour { get; set; }
        public decimal OTHour { get; set; }
        public int ApprovedOTHour { get; set; }

        public string EmployeeName { get; set; }
        public string EmpCode { get; set; }
        public string Designation { get; set; }

        public int DepartmentId { get; set; }
        public string Department { get; set; }

        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
