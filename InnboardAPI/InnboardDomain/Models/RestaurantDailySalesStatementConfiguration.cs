namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantDailySalesStatementConfiguration")]
    public partial class RestaurantDailySalesStatementConfiguration
    {
        [Key]
        public int DailySalesStatementId { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateTo { get; set; }

        public decimal PercentageAmount { get; set; }

        public decimal AmountInPercentage { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
