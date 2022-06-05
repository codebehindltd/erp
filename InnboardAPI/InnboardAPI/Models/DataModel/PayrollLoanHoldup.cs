namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollLoanHoldup")]
    public partial class PayrollLoanHoldup
    {
        [Key]
        public int LoanHoldupId { get; set; }

        public int LoanId { get; set; }

        public int EmpId { get; set; }

        [Column(TypeName = "date")]
        public DateTime LoanHoldupDateFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LoanHoldupDateTo { get; set; }

        public int InstallmentNumberWhenLoanHoldup { get; set; }

        public decimal DueAmount { get; set; }

        public decimal OverDueAmount { get; set; }

        public int? ApprovedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ApprovedDate { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }

        [Required]
        [StringLength(10)]
        public string HoldupStatus { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
