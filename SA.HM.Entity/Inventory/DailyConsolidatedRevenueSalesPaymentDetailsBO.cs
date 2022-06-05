using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class DailyConsolidatedRevenueSalesPaymentDetailsBO
    {
        public string PaymentMode  { get; set; }
        public string CardType { get; set; }
        public Nullable<int> PaymentCount { get; set; }
        public Nullable<decimal> PaymentAmount { get; set; }
    }
}
