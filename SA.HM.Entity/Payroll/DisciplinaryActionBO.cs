using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class DisciplinaryActionBO
    {
        public long DisciplinaryActionId { get; set; }
        public int DisciplinaryActionReasonId { get; set; }
        public int EmployeeId { get; set; }
        public short ActionTypeId { get; set; }
        public string ActionBody { get; set; }
        public int? ProposedActionId { get; set; }
        public DateTime? ApplicableDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public string EmpName { get; set; }
        public string ProposedAction { get; set; }
    }
}
