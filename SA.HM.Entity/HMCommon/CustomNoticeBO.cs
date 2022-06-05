using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.SalesAndMarketing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class CustomNoticeBO
    {
        public long Id { get; set; }
        public string NoticeName { get; set; }
        public string Content { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public bool? IsDeleted { get; set; }

        public List<EmployeeBO> EmployeeList { get; set; }
        public string EmployeeNameList { get; set; }
        public string AssignType { get; set; }
        public int? EmpDepartment { get; set; }
    }
}
