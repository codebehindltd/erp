using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InventoryCategoryWiseUsageNCostAnalysisBO
    {
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal? ActualUsage { get; set; }
        public decimal? ActualCost { get; set; }
        public decimal? TotalSalesAmount { get; set; }
        public decimal? CostByPercentage { get; set; }
    }
}
