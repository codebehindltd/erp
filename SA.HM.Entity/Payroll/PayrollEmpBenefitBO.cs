using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PayrollEmpBenefitBO
    {
        public long EmpBenefitMappingId { get; set; }
        public int EmpId { get; set; }
        public long BenefitHeadId { get; set; }
        public DateTime EffectiveDate { get; set; }

        public string BenefitName { get; set; }
        public string ShowEffectiveDate { get; set; }
        public string EmpName { get; set; }
        public string Designation { get; set; }
    }
}
