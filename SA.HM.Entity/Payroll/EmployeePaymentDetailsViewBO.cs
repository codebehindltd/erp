using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmployeePaymentDetailsViewBO
    {
        public Nullable<long> EmployeeBillId { get; set; }
        public long PaymentDetailsId { get; set; }
        public long PaymentId { get; set; }
        public Nullable<long> EmployeeBillDetailsId { get; set; }
        public long EmployeePaymentId { get; set; }
        public long BillId { get; set; }
        public decimal PaymentAmount { get; set; }
        public string BillNumber { get; set; }
        public string ModuleName { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public Nullable<decimal> DueAmount { get; set; }
    }
}
