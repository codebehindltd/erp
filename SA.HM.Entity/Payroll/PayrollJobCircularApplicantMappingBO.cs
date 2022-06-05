using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PayrollJobCircularApplicantMappingBO
    {
        public long JobCircularApplicantMappingId { get; set; }
        public long JobCircularId { get; set; }
        public long ApplicantId { get; set; }
        public string ApplicantType { get; set; }
        public string EmployeeName { get; set; }
    }
}
