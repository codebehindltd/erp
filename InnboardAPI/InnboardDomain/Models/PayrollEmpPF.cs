namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpPF")]
    public partial class PayrollEmpPF
    {
        [Key]
        public long PFCollectionId { get; set; }

        public int EmpId { get; set; }

        [Required]
        [StringLength(25)]
        public string PFType { get; set; }

        public decimal EmpContribution { get; set; }

        public decimal CompanyContribution { get; set; }

        public decimal ProvidentFundInterest { get; set; }

        public decimal CommulativeEmpContribution { get; set; }

        public decimal CommulativeCompanyContribution { get; set; }

        public decimal CommulativeInterestAmount { get; set; }

        public decimal CommulativePFAmountCurrentYear { get; set; }

        public decimal CommulativePFAmount { get; set; }

        public DateTime PFDateFrom { get; set; }

        public DateTime PFDateTo { get; set; }

        public short? PFYear { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
