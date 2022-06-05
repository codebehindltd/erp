namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollAppraisalRatingFactor")]
    public partial class PayrollAppraisalRatingFactor
    {
        [Key]
        public int RatingFactorId { get; set; }

        public int AppraisalIndicatorId { get; set; }

        [Required]
        [StringLength(250)]
        public string RatingFactorName { get; set; }

        public decimal RatingWeight { get; set; }

        [StringLength(150)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
