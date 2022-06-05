using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class AppraisalEvaluationDetailsViewBO
    {
        public AppraisalEvalutionByBO Master { get; set; }
        public List<AppraisalEvalutionRatingFactorDetailsBO> Details { get; set; }
        public List<AppraisalRatingScaleBO> RatingFactorScale { get; set; }
    }
}
