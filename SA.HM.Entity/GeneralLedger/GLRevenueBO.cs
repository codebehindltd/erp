using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLRevenueBO
    {
        public Nullable<long> Id { get; set; }
        public string Particulars { get; set; }
        public Nullable<short> SortingOrder { get; set; }
        public Nullable<int> GroupId { get; set; }
        public string GroupName { get; set; }
        public Nullable<int> YearId { get; set; }
        public Nullable<short> MonthId { get; set; }
        public string MonthsName { get; set; }
        public Nullable<int> SubId { get; set; }
        public string SubName { get; set; }
        public string NodeType { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> ProfitLoss { get; set; }
    }
}
