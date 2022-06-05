using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class AppraisalEvaluationReportViewBO
    {
        public int AppraisalEvalutionById { get; set; }
        public int  EmpId { get; set; }
        public string AppraisalType { get; set; }
        public DateTime EvaluationFromDate { get; set; }
        public string EvaluationFromDateForReport { get; set; }
        public DateTime EvaluationToDate { get; set; }
        public string EvaluationToDateForReport { get; set; }
        public string DisplayName { get; set; }
        public DateTime JoinDate { get; set; }
        public int Joblength { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string EmployeeType { get; set; }
        public decimal Marks { get; set; }
        public string EvaloatorName { get; set; }
        public decimal AllocatedMarks { get; set; }
        public decimal GainMarksRatingValue { get; set; }
    }
}
