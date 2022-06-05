using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Payroll
{
    public class AppraisalRatingScaleBO
    {
        public int RatingScaleId { get; set; }
        public string RatingScaleName { get; set; }
        public bool IsRemarksMandatory { get; set; }
        public Nullable<decimal> RatingValue { get; set; }
        public string RatingScaleNameWithRatingScale { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
