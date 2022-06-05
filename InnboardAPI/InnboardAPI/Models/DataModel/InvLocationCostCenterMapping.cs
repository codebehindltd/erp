namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvLocationCostCenterMapping")]
    public partial class InvLocationCostCenterMapping
    {
        [Key]
        public int MappingId { get; set; }

        public int? CostCenterId { get; set; }

        public int? LocationId { get; set; }
    }
}
