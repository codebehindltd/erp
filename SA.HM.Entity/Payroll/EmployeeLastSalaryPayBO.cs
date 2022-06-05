using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmployeeLastSalaryPayBO
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmpType { get; set; } 
        public string Position { get; set; }
        public string Location { get; set; }       
        public string Project { get; set; }  
    }
}
