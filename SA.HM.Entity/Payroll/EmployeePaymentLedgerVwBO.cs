using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmployeePaymentLedgerVwBO : EmployeePaymentLedgerBO
    {
        public string Narration { get; set; }
        public decimal? ClosingBalance { get; set; }
        public decimal? BalanceCommulative { get; set; }
        public string CurrencyName { get; set; }
        public string EmployeeBillNumber { get; set; }
        public int BillCurrencyId { get; set; }
    }
}
