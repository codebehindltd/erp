namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PayrollBonusSetting")]
    public partial class PayrollBonusSetting
    {
        [Key]
        public int BonusSettingId { get; set; }

        [Required]
        [StringLength(20)]
        public string BonusType { get; set; }

        public byte? EffectivePeriod { get; set; }

        public DateTime? BonusDate { get; set; }

        public decimal BonusAmount { get; set; }

        [Required]
        [StringLength(20)]
        public string AmountType { get; set; }

        public int DependsOnHead { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
