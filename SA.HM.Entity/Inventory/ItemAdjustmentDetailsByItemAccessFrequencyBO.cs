using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class ItemAdjustmentDetailsByItemAccessFrequencyBO
    {
        public Nullable<int> ItemId { get; set; }
        public string ItemName { get; set; }
        public Nullable<int> StockById { get; set; }
        public string StockBy { get; set; }
        public Nullable<decimal> OpeningStock { get; set; }
        public Nullable<decimal> ReceivedQuantity { get; set; }
        public Nullable<decimal> ActualUsageQuantity { get; set; }
        public Nullable<decimal> WastageQuantity { get; set; }
        public Nullable<decimal> StockQuantity { get; set; }
        public Nullable<decimal> StockQuantityAfterWastageDeduction { get; set; }
        public string AdjustmentFrequency { get; set; }
        public string ProductType { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int StyleId { get; set; }
        public string ColorText { get; set; }
        public string SizeText { get; set; }
        public string StyleText { get; set; }
    }
}
