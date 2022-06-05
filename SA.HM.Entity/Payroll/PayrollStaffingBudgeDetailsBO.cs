using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PayrollStaffingBudgeDetailsBO
    {
        public Int64 StaffingBudgetDetailsId { get; set; }
        public Int64 StaffingBudgetId { get; set; }
        public Int32 JobType { get; set; }
        public String JobLevel { get; set; }
        public Int16 NoOfStaff { get; set; }
        public Decimal BudgetAmount { get; set; }
        public String JobTypeName { get; set; }
        public Int32 DepartmentId { get; set; }
        public string Department { get; set; }
        public int? FiscalYear { get; set; }
        public string FiscalYearName { get; set; }
    }
}
