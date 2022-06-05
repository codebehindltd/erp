using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Entity.HouseKeeping
{
    public class TaskWiseEmployeeVWBO : EmpBankInfoBO
    {
        public long EmpTaskId { get; set; }
        public long TaskId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public int DepartmentId { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public string Department { get; set; }
        public int EmpTypeId { get; set; }
        public string EmpType { get; set; }
        public int DesignationId { get; set; }
        public string Designation { get; set; }       
    }

}
