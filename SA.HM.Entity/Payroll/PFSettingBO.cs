using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PFSettingBO
    {
        public int PFSettingId { get; set; }
        public decimal EmployeeContributionInPercentage { get; set; }
        public decimal CompanyContributionInPercentange { get; set; }
        public decimal EmployeeCanContributeMaxOfBasicSalary { get; set; }
        public decimal? InterestDistributionRate { get; set; }
        public int CompanyContributionEligibilityYear { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
