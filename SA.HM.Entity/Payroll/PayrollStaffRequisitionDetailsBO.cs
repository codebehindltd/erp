using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PayrollStaffRequisitionDetailsBO
    {
        public Int64 StaffRequisitionDetailsId { get; set; }
        public Int64 StaffRequisitionId { get; set; }
        public Int32 JobType { get; set; }
        public String JobLevel { get; set; }
        public Int16 RequisitionQuantity { get; set; }
        public decimal SalaryAmount { get; set; }
        public String JobTypeName { get; set; }
        public Int32 DepartmentId { get; set; }
        public string Department { get; set; }
        public DateTime DemandDate { get; set; }
        public string DemandDateString { get; set; }
        public string RequisitionDescription { get; set; }
        public int? FiscalYear { get; set; }
    }
}
