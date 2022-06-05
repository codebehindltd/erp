using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PayrollStaffRequisitionBO
    {
        public Int64 StaffRequisitionId { get; set; }
        public Int64 StaffRequisitionDetailsId { get; set; }
        public Int32 DepartmentId { get; set; }        
        public Int32 JobTypeId { get; set; }        
        public String ApprovedStatus { get; set; }
        public Int32 CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int32 LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public Int16 RequisitionQuantity { get; set; }
        public Decimal SalaryAmount { get; set; }
        public string JobLevel { get; set; }
        public String JobTypeName { get; set; }
        public string Department { get; set; }
        public int? FiscalYear { get; set; }
    }
}
