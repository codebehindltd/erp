using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class ServiceChargeConfigurationDetailsBO
    {
        public long ServiceChargeConfigurationDetailsId { get; set; }
        public long ServiceChargeConfigurationId { get; set; }
        public int EmpId { get; set; }

        public string DisplayName { get; set; }
        public string EmpCode { get; set; }
        public string Designation { get; set; }
        public decimal? ServiceAmount { get; set; }
    }
}
