namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvCategoryCostCenterMapping")]
    public partial class InvCategoryCostCenterMapping
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int MappingId { get; set; }

        public int? CostCenterId { get; set; }

        public int? CategoryId { get; set; }
    }
}
