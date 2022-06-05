using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class DtDMtDYtDMarketSegmentReportViewBO
    {
        public string MarketSegment { get; set; }
        public Nullable<int> ColumnGroupId { get; set; }
        public string ColumnGroup { get; set; }
        public Nullable<int> SubGroupId { get; set; }
        public string SubGroup { get; set; }
        public string QtNValue { get; set; }
    }
}
