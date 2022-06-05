using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class DailyConsolidatedRevenueCostCenterWiseSalesDetailsBO
    {
        public Nullable<int> CostCenterId { get; set; }
        public string CostCenter { get; set; }
        public Nullable<decimal> NetSales { get; set; }
        public Nullable<int> Guests { get; set; }
        public Nullable<decimal> Checks { get; set; }
        public Nullable<decimal> ChecksAmount { get; set; }
        public Nullable<decimal> AvgChecks { get; set; }
        public Nullable<decimal> AvgGuest { get; set; }
        public Nullable<decimal> NetSaleTTL { get; set; }
        public Nullable<decimal> GuestTTL { get; set; }
        public Nullable<decimal> ChecksTTL { get; set; }
        public Nullable<int> TablesUse { get; set; }
        public Nullable<decimal> TableTTL { get; set; }
        public Nullable<decimal> AvgTable { get; set; }
        public Nullable<decimal> Turns { get; set; }
    }
}
