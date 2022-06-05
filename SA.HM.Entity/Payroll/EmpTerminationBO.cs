using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PayrollEmpTerminationBO
    {
        public int TerminationId { get; set; }
        public int EmpId { get; set; }
        public System.DateTime DecisionDate { get; set; }
        public System.DateTime TerminationDate { get; set; }

        public int EmployeeStatusId { get; set; }
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }


    }
}
