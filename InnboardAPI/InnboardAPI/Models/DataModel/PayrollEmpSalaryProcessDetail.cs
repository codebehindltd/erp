namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpSalaryProcessDetail")]
    public partial class PayrollEmpSalaryProcessDetail
    {
        [Key]
        public int ProcessDetailId { get; set; }

        public int? ProcessId { get; set; }

        public int? EmpId { get; set; }

        public int? PayrollCurrencyId { get; set; }

        [Column(TypeName = "money")]
        public decimal? ConvertionRate { get; set; }

        public decimal? BasicAmountInEmployeeCurrency { get; set; }

        public decimal? BasicAmount { get; set; }

        public int? SalaryHeadId { get; set; }

        [StringLength(100)]
        public string SalaryHead { get; set; }

        [StringLength(100)]
        public string TransactionType { get; set; }

        [StringLength(250)]
        public string SalaryHeadNote { get; set; }

        public int? DependsOn { get; set; }

        [StringLength(25)]
        public string AddDeductDependsOn { get; set; }

        [StringLength(100)]
        public string SalaryType { get; set; }

        [StringLength(25)]
        public string SalaryEffectiveness { get; set; }

        [StringLength(100)]
        public string AmountType { get; set; }

        public decimal? Amount { get; set; }

        public decimal? SalaryAmount { get; set; }

        public decimal? GrossAmount { get; set; }

        public decimal? TotalAllowance { get; set; }

        public decimal? TotalDeduction { get; set; }

        public decimal? TotalAllowanceNotEffective { get; set; }

        public decimal? TotalDeductionNotEffective { get; set; }

        public decimal? HomeTakenAmount { get; set; }
    }
}
