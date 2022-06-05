using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class AppraisalEvaluationDetailsReportViewBO
    {
        public int AppraisalEvalutionById { get; set; }
        public int EmpId { get; set; }
        public string AppraisalType { get; set; }
        public string DisplayName { get; set; }
        public DateTime JoinDate { get; set; }
        public int Joblength { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string EmployeeType { get; set; }
        public decimal Marks { get; set; }
        public string AppraisalIndicatorName { get; set; }
        public decimal RatingWeight { get; set; }
        public string RatingFactorName { get; set; }
        public decimal RatingValue { get; set; }
        public decimal IndividualMarks { get; set; }
    }
}
