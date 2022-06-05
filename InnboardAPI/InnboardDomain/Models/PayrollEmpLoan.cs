namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpLoan")]
    public partial class PayrollEmpLoan
    {
        [Key]
        [Column(Order = 0)]
        public int LoanId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EmpId { get; set; }

        [Required]
        [StringLength(50)]
        public string LoanNumber { get; set; }

        [Required]
        [StringLength(25)]
        public string LoanType { get; set; }

        public decimal LoanAmount { get; set; }

        public decimal InterestRate { get; set; }

        public decimal InterestAmount { get; set; }

        public decimal DueAmount { get; set; }

        public decimal DueInterestAmount { get; set; }

        public int LoanTakenForPeriod { get; set; }

        [Required]
        [StringLength(20)]
        public string LoanTakenForMonthOrYear { get; set; }

        public decimal PerInstallLoanAmount { get; set; }

        public decimal PerInstallInterestAmount { get; set; }

        public DateTime LoanDate { get; set; }

        [StringLength(20)]
        public string LoanStatus { get; set; }

        [StringLength(15)]
        public string ApprovedStatus { get; set; }

        public int? CheckedBy { get; set; }

        public int? ApprovedBy { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
