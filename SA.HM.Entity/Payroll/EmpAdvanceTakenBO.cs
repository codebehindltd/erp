using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpAdvanceTakenBO
    {
        public int AdvanceId { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }

        public System.DateTime AdvanceDate { get; set; }
        public decimal AdvanceAmount { get; set; }
        public string PayMonth { get; set; }
        public bool IsDeductFromSalary { get; set; }
        public int CheckedBy { get; set; }  
        public int ApprovedBy { get; set; }
        public string ApprovedStatus { get; set; }
        public string ApproveEmpCode { get; set; }
        public string ApprovedEmployeeName { get; set; }        
        public string Remarks { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
