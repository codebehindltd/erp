using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class DailyConsolidatedRevenueCostCenterWiseNetSalesDetailsBO
    {
        public Nullable<long> TransactionId { get; set; }
        public Nullable<decimal> NetSales { get; set; }
        public Nullable<decimal> ServiceCharge { get; set; }
        public Nullable<decimal> TaxAmount { get; set; }
        public Nullable<decimal> TotalDiscounts { get; set; }
        public Nullable<int> ReturnTotal { get; set; }
        public Nullable<decimal> ReturnAmount { get; set; }
        public Nullable<int> VoidTotal { get; set; }
        public Nullable<decimal> VoidAmount { get; set; }
        public Nullable<decimal> GrandTotal { get; set; }
        public Nullable<decimal> TotalRevenue { get; set; }
        public Nullable<int> ErrorCorrects { get; set; }
        public Nullable<decimal> ErrorAmount { get; set; }
        public Nullable<int> Checks { get; set; }
        public Nullable<decimal> ChecksAmount { get; set; }
        public Nullable<int> ChecksPaid { get; set; }
        public Nullable<decimal> ChecksPaidAmount { get; set; }
        public Nullable<int> Outstanding { get; set; }
        public Nullable<decimal> OutstandingAmount { get; set; }
        public Nullable<int> TotalPax { get; set; }
        public Nullable<decimal> AvgChecks { get; set; }

    }
}
