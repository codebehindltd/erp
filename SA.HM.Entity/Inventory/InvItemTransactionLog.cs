using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvItemTransactionLog
    {
        public Int64 ItemTransactionId { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public Int64 TransactionForId { get; set; }
        public int ItemId { get; set; }
        public decimal DayOpeningQuantity { get; set; }
        public decimal TransactionalOpeningQuantity { get; set; }
        public decimal TransactionQuantity { get; set; }
        public decimal ClosingQuantity { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
