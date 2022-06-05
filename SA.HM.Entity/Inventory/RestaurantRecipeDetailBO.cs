using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class RestaurantRecipeDetailBO
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int ItemId { get; set; }
        public int RecipeItemId { get; set; }
        public string RecipeItemName { get; set; }
        public int UnitHeadId { get; set; }
        public string HeadName { get; set; }
        public decimal ItemUnit { get; set; }
        public decimal ItemCost { get; set; }
        public int TypeId { get; set; }
        public decimal AditionalCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal AverageCost { get; set; }
        public string AverageUnitHead { get; set; }
        public int AverageUnitHeadId { get; set; }
        public bool IsRecipe { get; set; }
        public bool IsGradientCanChange { get; set; }
        public string Status { get; set; }
        public int PreviousTypeId { get; set; }
        public decimal PreviousTotalCost { get; set; }

        public string Name { get; set; }
        public List<RestaurantRecipeDetailBO> RecipeModifierTypes { get; set; }
    }
}
