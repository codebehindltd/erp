using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class AppraisalEvalutionRatingFactorDetailsBO
    {
        public int RatingFacotrDetailsId { get; set; }
        public int AppraisalEvalutionById { get; set; }
        public int EmpId { get; set; }
        public int MarksIndicatorId { get; set; }
        public int AppraisalRatingFactorId { get; set; }
        public decimal AppraisalWeight { get; set; }
        public decimal RatingWeight { get; set; }
        public decimal RatingValue { get; set; }
        public decimal Marks { get; set; }
        public string Remarks { get; set; }
        public string RatingDropDownValue { get; set; }

        public decimal Weight { get; set; }
    }
}
