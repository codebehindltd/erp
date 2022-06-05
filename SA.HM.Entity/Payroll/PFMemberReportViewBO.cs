using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PFMemberReportViewBO
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Department { get; set; }
        public decimal? Basic { get; set; }
        public decimal? PFPercent { get; set; }
        public decimal? PFAmount { get; set; }
    }
}
