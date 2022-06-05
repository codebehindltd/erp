using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class GratuityMonthlyBalanceViewBO
    {
        public string Month { get; set; }
        public string Department { get; set; }
        public decimal? GratuityAmount { get; set; }

    }
}
