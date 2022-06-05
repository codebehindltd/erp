namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollEmpBenefit")]
    public partial class PayrollEmpBenefit
    {
        [Key]
        public long EmpBenefitMappingId { get; set; }

        public int EmpId { get; set; }

        public long BenefitHeadId { get; set; }

        public DateTime? EffectiveDate { get; set; }
    }
}
