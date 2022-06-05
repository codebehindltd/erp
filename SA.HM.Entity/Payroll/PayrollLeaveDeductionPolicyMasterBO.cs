using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PayrollLeaveDeductionPolicyMasterBO
    {
        public long Id { get; set; }
        public long NoOfLate { get; set; }
        public long NoOfLeave { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public List<PayrollLeaveDeductionPolicyDetailBO> DetailList { get; set; }
    }
}
