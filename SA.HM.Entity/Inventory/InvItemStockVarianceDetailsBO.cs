using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvItemStockVarianceDetailsBO
    {
        public int StockVarianceDetailsId { get; set; }
        public int StockVarianceId { get; set; }
        public int ItemId { get; set; }
        public int LocationId { get; set; }
        public int StockById { get; set; }
        public int TModeId { get; set; }
        public decimal UsageQuantity { get; set; }
        public decimal UsageCost { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal VarianceQuantity { get; set; }
        public decimal VarianceCost { get; set; }
        public string Reason { get; set; }

        public string TransactionMode { get; set; }
        public string StockByName { get; set; }
        public string LocationName { get; set; }
        public string ItemName { get; set; }

    }
}
