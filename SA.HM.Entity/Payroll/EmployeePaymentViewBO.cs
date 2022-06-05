using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmployeePaymentViewBO
    {
        public EmployeePaymentBO EmployeePayment { get; set; }
        public List<EmployeePaymentDetailsViewBO> EmployeePaymentDetails { get; set; }
        public EmployeeBO Employee = new EmployeeBO();
        public List<EmployeePaymentLedgerVwBO> EmployeeBill = new List<EmployeePaymentLedgerVwBO>();
        public List<EmployeePaymentLedgerVwBO> EmployeeGeneratedBill = new List<EmployeePaymentLedgerVwBO>();
    }
}
