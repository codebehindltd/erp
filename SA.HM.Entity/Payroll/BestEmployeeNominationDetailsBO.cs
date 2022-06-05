using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class BestEmployeeNominationDetailsBO
    {
        public long BestEmpNomineeDetailsId { get; set; }
        public long BestEmpNomineeId { get; set; }
        public string EmpId { get; set; }
        public bool? IsSelectedAsMonthlyBestEmployee { get; set; }
        public bool? IsSelectedForYearlyBestEmployee { get; set; }
        public bool? IsSelectedAsYearlyBestEmployee { get; set; }
    }
}
