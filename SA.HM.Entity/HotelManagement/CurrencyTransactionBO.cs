using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class CurrencyTransactionBO
    {
        public Nullable<System.DateTime> DateNTime { get; set; }
        public string InvoiceNumber { get; set; }
        public string Currency { get; set; }
        public Nullable<decimal> CurrencyAmount { get; set; }
        public Nullable<decimal> ConversionRate { get; set; }
        public Nullable<decimal> ConvertedAmount { get; set; }
        public string PaymentMode { get; set; }
        public string TransactionDetails { get; set; }
        public string Remarks { get; set; }
        public string ReceivedBy { get; set; }
        public string CostCenter { get; set; }
    }
}
