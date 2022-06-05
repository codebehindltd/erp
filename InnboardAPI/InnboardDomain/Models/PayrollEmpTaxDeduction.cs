namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpTaxDeduction")]
    public partial class PayrollEmpTaxDeduction
    {
        [Key]
        public long TaxCollectionId { get; set; }

        public int EmpId { get; set; }

        public decimal BasicAmount { get; set; }

        public decimal DeductionPercentage { get; set; }

        public decimal? RangeFrom { get; set; }

        public decimal? RangeTo { get; set; }

        public decimal TaxAmount { get; set; }

        public DateTime TaxDateFrom { get; set; }

        public DateTime TaxDateTo { get; set; }

        public short? TaxYear { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
