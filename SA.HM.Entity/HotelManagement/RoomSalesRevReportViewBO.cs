using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class RoomSalesRevReportViewBO
    {
        public int? Code { get; set; }
        public string ReportTitle { get; set; }
        public string ServiceName { get; set; }
        public decimal? RevenueAmount { get; set; }
    }
}
