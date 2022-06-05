using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class SettlementDetailsReportViewBO
    {
        public string PaymentMode { get; set; }
        public string Description { get; set; }
        public int ColumnGroupId { get; set; }
        public string ColumnGroup { get; set; }
        public int SubGroupId { get; set; }
        public string SubGroup { get; set; }
        public Nullable<decimal> QtNValue { get; set; }
    }
}
