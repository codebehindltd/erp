using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.LCManagement
{
    public class LCReportViewBO
    {
        public int LCId { get; set; }
        public string Supplier { get; set; }
        public string LCNumber { get; set; }
        public string LCOpenDate { get; set; }
        public string LCMatureDate { get; set; }
        public string LCSettlementDate { get; set; }
        public string ProductName { get; set; }
        public decimal? Quantity { get; set; }
        public string Stock { get; set; }
        public string ApprovedStatus { get; set; }
        public string Code { get; set; }
        public string TransactionType { get; set; }
        public string Name { get; set; }
        public string StockUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Total { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string ReceivedDateString { get; set; }
        public decimal? Amount { get; set; }
        public decimal? ReceiveAmount { get; set; }
        public decimal? PaymentAmount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string ExpenseDateString { get; set; }
        public string PaymentBy { get; set; }
        public string Description { get; set; }



        public string PINumber { get; set; }
        public string LCType { get; set; }
        public string BankAccount { get; set; }
        public string LCValue { get; set; }
        public string LCOpenDateString { get; set; }
        public string LCMatureDateString { get; set; }
        public string SupplierInfo { get; set; }
        public string Remarks { get; set; }
        public string UserName { get; set; }
        public string CreatedDateString { get; set; }
        public string SettlementByName { get; set; }
        public string SettlementDateString { get; set; }
        public string SettlementDescription { get; set; }
    }
}
