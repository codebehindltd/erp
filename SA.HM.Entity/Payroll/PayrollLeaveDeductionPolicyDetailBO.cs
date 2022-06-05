using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PayrollLeaveDeductionPolicyDetailBO
    {
        public long Id { get; set; }
        public long MasterId { get; set; }
        public int LeaveId { get; set; }
        public string LeaveName { get; set; }
        public int? Sequence { get; set; }
    }
}
