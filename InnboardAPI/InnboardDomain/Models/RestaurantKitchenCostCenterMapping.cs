namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantKitchenCostCenterMapping")]
    public partial class RestaurantKitchenCostCenterMapping
    {
        [Key]
        public long MappingId { get; set; }

        public int CostCenterId { get; set; }

        public int KitchenId { get; set; }
    }
}
