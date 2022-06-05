namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PayrollAppraisalEvalutionRatingFactorDetails
    {
        [Key]
        public int RatingFacotrDetailsId { get; set; }

        public int AppraisalEvalutionById { get; set; }

        public int MarksIndicatorId { get; set; }

        public int AppraisalRatingFactorId { get; set; }

        public decimal AppraisalWeight { get; set; }

        public decimal RatingWeight { get; set; }

        public decimal? RatingValue { get; set; }

        public decimal Marks { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
