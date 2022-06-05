using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class ServiceChargeDistributionDetailsViewBO : ServiceChargeDistributionDetailsBO
    {
        public DateTime ProcessDateFrom { get; set; }
        public DateTime ProcessDateTo { get; set; }
        public string EmployeeName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Designation { get; set; }
        public string ReportProcessDate { get; set; }

        public string AccountNumber { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
    }
}
