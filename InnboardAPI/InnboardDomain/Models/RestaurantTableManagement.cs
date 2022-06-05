namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantTableManagement")]
    public partial class RestaurantTableManagement
    {
        [Key]
        public int TableManagementId { get; set; }

        public int? CostCenterId { get; set; }

        public int TableId { get; set; }

        public double? XCoordinate { get; set; }

        public double? YCoordinate { get; set; }

        public int? TableWidth { get; set; }

        public int? TableHeight { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [Required]
        [StringLength(500)]
        public string DivTransition { get; set; }
    }
}
