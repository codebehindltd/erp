using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class MonthWiseMarketSegmentReportViewBO
    {
        public string MarketSegment { get; set; }
        public string ServiceMonth { get; set; }
        public Nullable<decimal> TotalRoomCharge { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> RoomRate { get; set; }
        public string Value { get; set; }

        public string ReferenceName { get; set; }
        public Nullable<int> Nights { get; set; }
        public Nullable<int> RoomCount { get; set; }
    }
}
