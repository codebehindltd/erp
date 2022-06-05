using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class AppraisalEvaluationViewBO
    {
        public int AppraisalEvalutionById { get; set; }
        public int? EmpId { get; set; }
        public string EmployeeName { get; set; }
        public decimal? TotalMarks { get; set; }
        public string MarksOutOf { get; set; }
        public string EvalutiorName { get; set; }
        public string AppraisalDuration { get; set; }
        public string EvalutionCompletionByString { get; set; }
}
}
