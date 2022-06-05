namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollTaxSetting")]
    public partial class PayrollTaxSetting
    {
        [Key]
        public int TaxSettingId { get; set; }

        public decimal TaxBandForMale { get; set; }

        public decimal TaxBandForFemale { get; set; }

        public bool? IsTaxPaidByCompany { get; set; }

        [StringLength(50)]
        public string CompanyContributionType { get; set; }

        public decimal? CompanyContributionAmount { get; set; }

        public bool? IsTaxDeductFromSalary { get; set; }

        [StringLength(50)]
        public string EmpContributionType { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
