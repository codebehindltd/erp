namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollSalaryFormula")]
    public partial class PayrollSalaryFormula
    {
        [Key]
        public int FormulaId { get; set; }

        [StringLength(50)]
        public string TransactionType { get; set; }

        public int? GradeIdOrEmployeeId { get; set; }

        public int? SalaryHeadId { get; set; }

        public int? DependsOn { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(20)]
        public string AmountType { get; set; }

        public bool? IsBasicOrGross { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
