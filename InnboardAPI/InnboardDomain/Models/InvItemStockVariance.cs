namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvItemStockVariance")]
    public partial class InvItemStockVariance
    {
        [Key]
        public int StockVarianceId { get; set; }

        public DateTime StockVarianceDate { get; set; }

        public int CostCenterId { get; set; }

        [Required]
        [StringLength(15)]
        public string ApprovedStatus { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
