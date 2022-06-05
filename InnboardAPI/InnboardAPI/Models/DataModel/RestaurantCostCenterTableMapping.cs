namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantCostCenterTableMapping")]
    public partial class RestaurantCostCenterTableMapping
    {
        [Key]
        public int MappingId { get; set; }

        public int? CostCenterId { get; set; }

        public int? TableId { get; set; }

        public int? StatusId { get; set; }
    }
}
