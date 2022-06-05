namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvItemStockAdjustment")]
    public partial class InvItemStockAdjustment
    {
        [Key]
        public int StockAdjustmentId { get; set; }

        public DateTime AdjustmentDate { get; set; }

        public int CostCenterId { get; set; }

        [StringLength(50)]
        public string AdjustmentFrequency { get; set; }

        [Required]
        [StringLength(15)]
        public string ApprovedStatus { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? LocationId { get; set; }
    }
}
