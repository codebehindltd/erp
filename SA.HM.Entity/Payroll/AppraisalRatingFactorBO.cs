using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class AppraisalRatingFactorBO
    {
        public int RatingFactorId { get; set; }
        public int AppraisalIndicatorId { get; set; }
        public string AppraisalIndicatorName { get; set; }
        public string RatingFactorName { get; set; }
        public decimal RatingWeight { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        public int RatingFacotrDetailsId { get; set; }
    }
}
