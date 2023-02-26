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
        public string RecipeItemCodeAndName { get; set; }
        public decimal ItemUnit { get; set; }
        public decimal ItemCost { get; set; }
        public string HeadName { get; set; }
        public string PreparationInstructions { get; set; }
        public string ServingInstructions { get; set; }
        public string IngredientPrepComments { get; set; }
        public long ItemRank { get; set; }        
        public string FinishedGoodsName { get; set; }
        public string FinishedGoodsCode { get; set; }
        public long NTypeId { get; set; }
        public string NutrientTypeName { get; set; }
        public string NutrientTypeCode { get; set; }
        public long NutrientsId { get; set; }
        public string NutrientName { get; set; }
        public string NutrientCode { get; set; }
        public decimal RequiredValue { get; set; }
        public decimal CalculatedFormula { get; set; }
        public decimal CalculatedValue { get; set; }
        public string FinishedGoodsCodeAndName { get; set; }
        public decimal TotalRawMaterialsCost { get; set; }
        public decimal TotalOverheadCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal SalesRate { get; set; }
        public decimal ProfitAndLoss { get; set; }            

    }
}
