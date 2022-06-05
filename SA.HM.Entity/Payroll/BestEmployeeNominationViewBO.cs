using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class BestEmployeeNominationViewBO : BestEmployeeNominationBO
    {
        public long BestEmpNomineeDetailsId { get; set; }
        public Int32 EmpId { get; set; }
        public bool? IsSelectedAsMonthlyBestEmployee { get; set; }
        public bool? IsSelectedAsYearlyBestEmployee { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string DepartmentName { get; set; }
    }
}
