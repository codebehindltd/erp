using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class PayrollApplicantResultBO
    {
        public long ApplicantResultId { get; set; }
        public long JobCircularId { get; set; }
        public long ApplicantId { get; set; }
        public short InterviewTypeId { get; set; }
        public decimal MarksObtain { get; set; }
        public string Remarks { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public string JobTitle { get; set; }
        public string ApplicantName { get; set; }
        public string InterviewName { get; set; }
        public decimal TotalMarks { get; set; }
    }
}
