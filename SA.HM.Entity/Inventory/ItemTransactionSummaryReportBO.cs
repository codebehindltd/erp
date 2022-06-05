using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class ItemTransactionSummaryReportBO
    {
        public long ItemTransactionId { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public Nullable<int> LocationId { get; set; }
        public int ItemId { get; set; }
        public Nullable<decimal> AverageCost { get; set; }
        public decimal DayOpeningQuantity { get; set; }
        public decimal TransactionalOpeningQuantity { get; set; }
        public Nullable<decimal> ReceiveQuantity { get; set; }
        public Nullable<decimal> OutItemQuantity { get; set; }
        public Nullable<decimal> WastageQuantity { get; set; }
        public Nullable<decimal> AdjustmentQuantity { get; set; }
        public Nullable<decimal> SalesQuantity { get; set; }
        public decimal ClosingQuantity { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ItemName { get; set; }
        public string HeadName { get; set; }
        public string LocationName { get; set; }
    }
}
