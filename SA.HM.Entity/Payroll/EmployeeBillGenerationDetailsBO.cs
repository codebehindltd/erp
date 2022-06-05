using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmployeeBillGenerationDetailsBO
    {
        public long EmployeeBillDetailsId { get; set; }
        public long EmployeeBillId { get; set; }
        public long EmployeePaymentId { get; set; }
        public int BillId { get; set; }
        public decimal Amount { get; set; }
    }
}
