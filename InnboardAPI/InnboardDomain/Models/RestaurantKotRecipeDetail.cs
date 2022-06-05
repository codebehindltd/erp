namespace InnboardDomain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantKotRecipeDetail")]
    public partial class RestaurantKotRecipeDetail
    {
        [Key]
        public int RecipeId { get; set; }

        public int? KotId { get; set; }

        public int? ItemId { get; set; }

        public int? RecipeItemId { get; set; }

        [StringLength(200)]
        public string RecipeItemName { get; set; }

        public int? UnitHeadId { get; set; }

        public decimal? ItemUnit { get; set; }

        public decimal? ItemCost { get; set; }

        [StringLength(50)]
        public string HeadName { get; set; }

        public int? TypeId { get; set; }

        [StringLength(50)]
        public string Status { get; set; }
    }
}
