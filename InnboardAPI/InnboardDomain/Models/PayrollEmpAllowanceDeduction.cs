namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpAllowanceDeduction")]
    public partial class PayrollEmpAllowanceDeduction
    {
        [Key]
        public int EmpAllowDeductId { get; set; }

        [Required]
        [StringLength(20)]
        public string AllowDeductType { get; set; }

        public int? DepartmentId { get; set; }

        public int? EmpId { get; set; }

        public int SalaryHeadId { get; set; }

        [StringLength(20)]
        public string AmountType { get; set; }

        [StringLength(25)]
        public string DependsOn { get; set; }

        public decimal? AllowDeductAmount { get; set; }

        public DateTime? EffectFrom { get; set; }

        public DateTime? EffectTo { get; set; }

        public int? EffectiveYear { get; set; }

        [Column(TypeName = "text")]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
