using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmployeePaymentDetailsBO
    {
        public Int64 PaymentDetailsId { get; set; }
        public Int64 PaymentId { get; set; }
        public Int64 EmployeeBillDetailsId { get; set; }
        public Int64 EmployeePaymentId { get; set; }
        public int BillId { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}
