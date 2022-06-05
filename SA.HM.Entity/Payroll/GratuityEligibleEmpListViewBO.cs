using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class GratuityEligibleEmpListViewBO
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Department { get; set; }
        public decimal? Basic { get; set; }
        public decimal? GratuityAmount { get; set; }
    }
}
