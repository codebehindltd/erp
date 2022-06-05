using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PayrollStaffingBudgetViewBO
    {
        public PayrollStaffingBudgetBO StaffingBudget { get; set; }
        public List<PayrollStaffingBudgeDetailsBO> StaffingBudgetDetails { get; set; }
    }
}
