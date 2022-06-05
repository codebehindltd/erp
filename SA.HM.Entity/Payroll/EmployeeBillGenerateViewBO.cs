using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmployeeBillGenerateViewBO
    {
        public Int64 EmployeeBillId { get; set; }
        public Int64 EmployeeBillDetailsId { get; set; }
        public Int32 EmployeeId { get; set; }
        public DateTime BillDate { get; set; }
        public string EmployeeBillNumber { get; set; }
        public Int64 EmployeePaymentId { get; set; }
        public Int32 BillId { get; set; }
        public decimal Amount { get; set; }
        public string BillNumber { get; set; }
        public string ModuleName { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal DueAmount { get; set; }

        public Int64 PaymentDetailsId { get; set; }

        public string EmployeeName { get; set; }
    }
}
