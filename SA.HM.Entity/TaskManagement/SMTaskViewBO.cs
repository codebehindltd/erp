using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.SalesAndMarketing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.TaskManagement
{
    public class SMTaskViewBO
    {
        public List<SMTask> TaskList { get; set; }
        public SMTask Task { get; set; }
        public List<EmployeeBO> EmployeeList { get; set; }
        public List<long> ContactLIst { get; set; }
        public TMTaskAssignedEmployee Employee { get; set; }
    }
}
