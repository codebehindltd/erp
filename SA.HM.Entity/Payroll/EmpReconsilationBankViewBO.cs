using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpReconsilationBankViewBO
    {
        public int BankId { get; set; }
        public string BankName { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
