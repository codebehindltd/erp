using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PFMonthlyTotalAmountViewBO
    {
        public string Month { get; set; }
        public string Department { get; set; }
        public decimal? EmpContribution { get; set; }
        public decimal? CmpContribution { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
