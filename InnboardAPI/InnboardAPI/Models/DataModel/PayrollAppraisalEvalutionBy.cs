namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollAppraisalEvalutionBy")]
    public partial class PayrollAppraisalEvalutionBy
    {
        [Key]
        public int AppraisalEvalutionById { get; set; }

        [Required]
        [StringLength(15)]
        public string AppraisalConfigType { get; set; }

        public int EvalutiorId { get; set; }

        public int? EmpId { get; set; }

        public DateTime? EvalutionCompletionBy { get; set; }

        [Required]
        [StringLength(50)]
        public string AppraisalType { get; set; }

        [Column(TypeName = "date")]
        public DateTime EvaluationFromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EvaluationToDate { get; set; }

        [StringLength(25)]
        public string ApprovalStatus { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
