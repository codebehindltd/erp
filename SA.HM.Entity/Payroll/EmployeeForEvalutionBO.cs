using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class EmployeeForEvalutionBO
    {
        public int AppraisalEvalutionById { get; set; }

        public Int64 SerialNumber { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public string AppraisalType { get; set; }
        public DateTime EvaluationFromDate { get; set; }
        public DateTime EvaluationToDate { get; set; }
        public DateTime EvalutionCompletionBy { get; set; }
        
    }
}
