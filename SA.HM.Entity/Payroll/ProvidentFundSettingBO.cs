using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class ProvidentFundSettingBO
    {
        public int PFSettingId { get; set; }
        public int EmployeeContributionInPercentage { get; set; }
        public int CompanyContributionInPercentange { get; set; }
        public int EmployeeCanContributeMaxOfBasicSalary { get; set; }
        public Nullable<decimal> InterestDistributionRate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
