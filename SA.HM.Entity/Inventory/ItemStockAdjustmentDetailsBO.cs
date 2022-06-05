using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class ItemStockAdjustmentDetailsBO
    {
        public int StockAdjustmentDetailsId { get; set; }
        public int StockAdjustmentId { get; set; }
        public int ItemId { get; set; }
        public int LocationId { get; set; }
        public int StockById { get; set; }
        public decimal OpeningQuantity { get; set; }
        public decimal ReceiveQuantity { get; set; }
        public decimal ActualUsage { get; set; }
        public decimal WastageQuantity { get; set; }
        public decimal StockQuantity { get; set; }
        public decimal ActualQuantity { get; set; }

        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public string LocationName { get; set; }
        public string StockByName { get; set; }
        public string TransactionMode { get; set; }

        public DateTime AdjustmentDate { get; set; }
        public string AdjustmentDateString { get; set; }
        public int CostCenterId { get; set; }
        public string CostCenter { get; set; }
        public string AdjustmentFrequency { get; set; }
        public string Location { get; set; }
        public string UnitHead { get; set; }
        public decimal UsageQuantity { get; set; }
        public decimal StockVariance { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal VarianceCost { get; set; }
        public string WastageType { get; set; }
        public string Reason { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int StyleId { get; set; }
        public string ColorText { get; set; }
        public string SizeText { get; set; }
        public string StyleText { get; set; }
    }
}
