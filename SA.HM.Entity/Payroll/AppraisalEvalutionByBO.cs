using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class AppraisalEvalutionByBO
    {
        public int AppraisalEvalutionById { get; set; }
        public string AppraisalConfigType { get; set; }
        public int EvalutiorId { get; set; }
        public string EmployeeCode { get; set; }
        public int EmpId { get; set; }
        public string EvalutorCode { get; set; }
        public string EvalutorName { get; set; }
        public DateTime EvalutionCompletionBy { get; set; }
        public string AppraisalType { get; set; }
        public DateTime EvaluationFromDate { get; set; }
        public DateTime EvaluationToDate { get; set; }
        public string ApprovalStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public int DepartmentId { get; set; }
    }
}
