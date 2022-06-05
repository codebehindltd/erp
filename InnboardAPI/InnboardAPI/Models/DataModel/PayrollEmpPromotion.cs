namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpPromotion")]
    public partial class PayrollEmpPromotion
    {
        [Key]
        public long PromotionId { get; set; }

        public int EmpId { get; set; }

        public DateTime PromotionDate { get; set; }

        public int PreviousDesignationId { get; set; }

        public int PreviousGradeId { get; set; }

        public int CurrentDesignationId { get; set; }

        public int CurrentGradeId { get; set; }

        [Required]
        [StringLength(25)]
        public string ApprovalStatus { get; set; }

        [Column(TypeName = "text")]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
