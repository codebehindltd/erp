using HotelManagement.Entity.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.DocumentManagement
{
    public class DMDocumentBO
    {
        public long Id { get; set; }
        public string DocumentName { get; set; }
        public string Description { get; set; }
        public string EmailReminderType { get; set; }
        public bool IsEmailReminderSent { get; set; }
        public DateTime? EmailReminderDate { get; set; }
        public DateTime? EmailReminderTime { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string CallToAction { get; set; }

        public List<EmployeeBO> EmployeeList { get; set; }
        public string EmployeeNameList { get; set; }
        public string AssignType { get; set; }
        public int? EmpDepartment { get; set; }
    }
}
