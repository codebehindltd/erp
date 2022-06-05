using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class OvertimeAnalysisBO
    {
        public Nullable<int> EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<decimal> TotalOTHour { get; set; }
        public Nullable<decimal> OTRate { get; set; }
        public Nullable<decimal> OTAmount { get; set; }
        public string Designation { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string WorkStationName { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<int> DonorId { get; set; }
        public string Project { get; set; }
    }
}
