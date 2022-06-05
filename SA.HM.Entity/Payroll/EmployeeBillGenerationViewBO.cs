using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmployeeBillGenerationViewBO
    {
        public EmployeeBillGenerationBO BillGeneration { get; set; }
        public List<EmployeeBillGenerationDetailsBO> BillGenerationDetails { get; set; }
        public List<EmployeePaymentLedgerVwBO> EmployeeBill = new List<EmployeePaymentLedgerVwBO>();
    }
}
