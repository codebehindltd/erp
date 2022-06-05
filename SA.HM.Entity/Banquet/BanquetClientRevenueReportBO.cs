using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Banquet
{
    public class BanquetClientRevenueReportBO
    {
        public string ReportTitle { get; set; }
        public string HeadName { get; set; }
        public decimal? TotalAmount { get; set; }
        public string ServiceName { get; set; }
        public string MonthName { get; set; }
        public int? MonthValue { get; set; }
    }
}
