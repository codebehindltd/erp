using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class ServiceChargeDistributionDetailsBO
    {
        public long ServiceProcessDetailsId { get; set; }
        public long ServiceProcessId { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public byte TotalAttendance { get; set; }
        public byte ServiceDays { get; set; }
        public decimal ServiceAmount { get; set; }
    }
}
