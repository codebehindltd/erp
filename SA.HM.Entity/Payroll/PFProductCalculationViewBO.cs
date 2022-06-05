using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PFProductCalculationViewBO
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public decimal? OpeningBalance { get; set; }
        public decimal? PFTotal { get; set; }
        public decimal? TotalProduct { get; set; }

        public decimal? Interest { get; set; }
    }
}
