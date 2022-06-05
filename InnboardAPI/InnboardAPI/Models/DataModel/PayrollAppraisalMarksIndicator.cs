namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollAppraisalMarksIndicator")]
    public partial class PayrollAppraisalMarksIndicator
    {
        [Key]
        public int MarksIndicatorId { get; set; }

        [Required]
        [StringLength(150)]
        public string AppraisalIndicatorName { get; set; }

        public decimal AppraisalWeight { get; set; }

        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
