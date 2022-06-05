namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RestaurantRecipeDetail")]
    public partial class RestaurantRecipeDetail
    {
        [Key]
        public int RecipeId { get; set; }

        public int? ItemId { get; set; }

        public int? RecipeItemId { get; set; }

        [StringLength(200)]
        public string RecipeItemName { get; set; }

        public int? UnitHeadId { get; set; }

        public decimal? ItemUnit { get; set; }

        public decimal? ItemCost { get; set; }

        public bool? IsRecipe { get; set; }
    }
}
