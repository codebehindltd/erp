using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PayrollDisciplinaryActionBO
    {
        public long DisciplinaryActionId { get; set; }
        public int DisciplinaryActionReasonId { get; set; }
        public int EmployeeId { get; set; }
        public short ActionTypeId { get; set; }
        public string ActionBody { get; set; }
        public string ProposedAction { get; set; }
        public Nullable<System.DateTime> ApplicableDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
