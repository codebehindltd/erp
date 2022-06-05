using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmpCareerInfoBO
    {
        public int CareerInfoId { get; set; }
        public int? EmpId { get; set; }
        public string Objective { get; set; }
        public decimal? PresentSalary { get; set; }
        public decimal? ExpectedSalary { get; set; }
        public int Currency { get; set; }
        public string JobLevel { get; set; }
        public string AvailableType { get; set; }
        public int? PreferedJobType { get; set; }
        public int? PreferedOrganizationType { get; set; }
        public string CareerSummary { get; set; }
        public int? PreferedJobLocationId { get; set; }
        public string ExtraCurriculmActivities { get; set; }
        public string PreferedJobLocation { get; set; }
        public string PreferedJobCategoryText { get; set; }
        public string PreferedOrganizationText { get; set; }
    }
}
