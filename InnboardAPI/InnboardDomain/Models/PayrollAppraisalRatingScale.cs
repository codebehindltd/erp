namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollAppraisalRatingScale")]
    public partial class PayrollAppraisalRatingScale
    {
        [Key]
        public int RatingScaleId { get; set; }

        [Required]
        [StringLength(50)]
        public string RatingScaleName { get; set; }

        public bool IsRemarksMandatory { get; set; }

        public decimal? RatingValue { get; set; }

        [StringLength(150)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
