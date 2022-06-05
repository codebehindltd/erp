namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollBenefitHead")]
    public partial class PayrollBenefitHead
    {
        [Key]
        public long BenefitHeadId { get; set; }

        [StringLength(500)]
        public string BenefitName { get; set; }
    }
}
