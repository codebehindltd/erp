namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollPFSetting")]
    public partial class PayrollPFSetting
    {
        [Key]
        public int PFSettingId { get; set; }

        [StringLength(50)]
        public string PFDependsOn { get; set; }

        [StringLength(50)]
        public string AmountType { get; set; }

        public decimal EmpContributionInPercentage { get; set; }

        public decimal CompanyContributionInPercentange { get; set; }

        public decimal EmpCanContributeMaxOfBasicSalary { get; set; }

        public decimal? InterestDistributionRate { get; set; }

        public int? CompanyContributionEligibilityYear { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
