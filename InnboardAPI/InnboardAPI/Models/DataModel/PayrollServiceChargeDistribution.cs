namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollServiceChargeDistribution")]
    public partial class PayrollServiceChargeDistribution
    {
        [Key]
        public long ServiceProcessId { get; set; }

        public DateTime? ProcessDateFrom { get; set; }

        public DateTime? ProcessDateTo { get; set; }

        public int? ProcessYear { get; set; }

        public decimal? DistributionPercentage { get; set; }

        public decimal ServiceAmount { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
