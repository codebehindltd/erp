using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PayrollStaffingBudgetByDepartmentIdBO
    {
        public long StaffingBudgetId { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<int> NumberOfStaff { get; set; }
        public Nullable<decimal> TotalBudget { get; set; }
    }
}
