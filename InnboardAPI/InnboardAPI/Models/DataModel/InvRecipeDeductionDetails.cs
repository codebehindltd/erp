namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InvRecipeDeductionDetails
    {
        [Key]
        public long ReceipeDeductionId { get; set; }

        [Required]
        [StringLength(50)]
        public string TransactionType { get; set; }

        public int? FinishProductRBillId { get; set; }

        public int? ItemIdMain { get; set; }

        public string ItemMainName { get; set; }

        public int? ItemId { get; set; }

        public int? RowIndex { get; set; }

        public int? RecipeItemId { get; set; }

        public string RecipeItemName { get; set; }

        public int? UnitHeadId { get; set; }

        public decimal? ItemUnit { get; set; }

        public decimal? ConvertionUnit { get; set; }

        public decimal? TotalUnitWillDeduct { get; set; }

        public decimal? UnitDeduction { get; set; }

        public decimal? RecipeDeduction { get; set; }

        public decimal? UnitDiffernce { get; set; }

        public decimal? QuantityMain { get; set; }

        public decimal? ParentQuantity { get; set; }

        public decimal? StockQuantity { get; set; }

        public bool? IsRecipeExist { get; set; }

        public bool? IsRecipe { get; set; }

        public DateTime? DeductionDate { get; set; }

        public decimal? AverageCost { get; set; }

        public decimal? DeductionQuantity { get; set; }

        public decimal? TotalCost { get; set; }

        public int LocationId { get; set; }
    }
}
