namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMBillingPeriod")]
    public partial class SMBillingPeriod
    {
        [Key]
        public int BillingPeriodId { get; set; }

        [Required]
        [StringLength(250)]
        public string BillingPeriodName { get; set; }

        public short BillingPeriodValue { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
