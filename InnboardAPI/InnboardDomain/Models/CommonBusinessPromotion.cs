namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonBusinessPromotion")]
    public partial class CommonBusinessPromotion
    {
        [Key]
        public int BusinessPromotionId { get; set; }

        [StringLength(250)]
        public string BPHead { get; set; }

        public DateTime? PeriodFrom { get; set; }

        public DateTime? PeriodTo { get; set; }

        [StringLength(50)]
        public string TransactionType { get; set; }

        public bool? IsBPPublic { get; set; }

        public decimal? PercentAmount { get; set; }

        public bool? ActiveStat { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
