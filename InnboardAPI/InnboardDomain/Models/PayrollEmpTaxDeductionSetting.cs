namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpTaxDeductionSetting")]
    public partial class PayrollEmpTaxDeductionSetting
    {
        [Key]
        public int TaxDeductionId { get; set; }

        public decimal RangeFrom { get; set; }

        public decimal RangeTo { get; set; }

        public decimal DeductionPercentage { get; set; }

        [Required]
        [StringLength(50)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
