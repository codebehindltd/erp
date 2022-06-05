using HotelManagement.Entity.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class TemplateEmailBO : TemplateInformationBO
    {
        //public long Id { get; set; }
        public long TemplateId { get; set; }
        public string TemplateBody { get; set; }
        public string TemplateType { get; set; }
        public string DisplayName { get; set; }
        public string AssignType { get; set; }

        public List<EmployeeBO> EmployeeList { get; set; }
        //public int CreatedBy { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public int? LastModifiedBy { get; set; }
        //public DateTime? LastModifiedDate { get; set; }
    }
}
