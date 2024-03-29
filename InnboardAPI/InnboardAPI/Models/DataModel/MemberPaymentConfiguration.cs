namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemberPaymentConfiguration")]
    public partial class MemberPaymentConfiguration
    {
        [Key]
        public long MemPaymentConfigId { get; set; }

        [StringLength(50)]
        public string TransactionType { get; set; }

        public long MemberTypeOrMemberId { get; set; }

        [StringLength(50)]
        public string BillingPeriod { get; set; }

        public decimal? BillingAmount { get; set; }

        public DateTime? BillingStartDate { get; set; }

        public DateTime? DoorAccessDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
