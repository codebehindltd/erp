namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantKotBillMaster")]
    public partial class RestaurantKotBillMaster
    {
        [Key]
        public int KotId { get; set; }

        public DateTime? KotDate { get; set; }

        public int? BearerId { get; set; }

        public int? CostCenterId { get; set; }

        [StringLength(100)]
        public string SourceName { get; set; }

        public int? SourceId { get; set; }

        public int? PaxQuantity { get; set; }

        public decimal? TotalAmount { get; set; }

        public bool? IsBillProcessed { get; set; }

        [StringLength(50)]
        public string KotStatus { get; set; }

        [StringLength(300)]
        public string Remarks { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [StringLength(25)]
        public string TokenNumber { get; set; }

        public bool? IsBillHoldup { get; set; }

        public bool? IsKotReturn { get; set; }

        public int? ReferenceKotId { get; set; }
    }
}
