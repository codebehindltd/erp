namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ReceipeModifierType")]
    public partial class ReceipeModifierType
    {
        public int Id { get; set; }

        public int? ItemId { get; set; }

        public int? RecipeItemId { get; set; }

        [StringLength(50)]
        public string UnitHead { get; set; }

        public decimal? UnitQuantity { get; set; }

        public decimal? AdditionalCost { get; set; }

        public decimal? TotalCost { get; set; }

        public int? UnitHeadId { get; set; }
    }
}
