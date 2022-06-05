using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmployeeAdvanceTakenBO
    {
        public int AdvanceId { get; set; }
        public int EmpId { get; set; }
        public System.DateTime AdvanceDate { get; set; }
        public decimal AdvanceAmount { get; set; }
        public string PayMonth { get; set; }
        public bool IsDeductFromSalary { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
