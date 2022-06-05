using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvItemTransactionBO
    {
        public long TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string StartBillNumber { get; set; }
        public string EndingBillNumber { get; set; }
        public Int16? TotalBillCount { get; set; }
        public Int16? TotalVoidQuantity { get; set; }
        public decimal? GrossSalesAmount { get; set; }
        public decimal? TotalVatAmount { get; set; }
        public decimal? TotalServiceChargeAmount { get; set; }
        public decimal? TotalNetSalesAmount { get; set; }
    }
}
