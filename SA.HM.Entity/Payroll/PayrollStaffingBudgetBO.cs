using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PayrollStaffingBudgetBO
    {
        public Int64 StaffingBudgetId { get; set; }
        public Int32 DepartmentId { get; set; }
        public String ApprovedStatus { get; set; }
        public Int32 CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int32 LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public Int16 NoOfStaff { get; set; }
        public Decimal BudgetAmount { get; set; }
        public string JobLevel { get; set; }
        public String JobTypeName { get; set; }
        public string Department { get; set; }
        public string FiscalYear { get; set; }
    }
}
