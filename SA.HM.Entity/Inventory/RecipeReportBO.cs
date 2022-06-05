using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class RecipeReportBO
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int RecipeItemId { get; set; }
        public string RecipeItemName { get; set; }
        public decimal ItemUnit { get; set; }
        public decimal ItemCost { get; set; }
        public string HeadName { get; set; }
        public string PreparationInstructions { get; set; }
        public string ServingInstructions { get; set; }
        public string IngredientPrepComments { get; set; }
        public long ItemRank { get; set; }
    }
}
