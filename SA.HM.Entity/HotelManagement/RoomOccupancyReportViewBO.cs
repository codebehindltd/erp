using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class RoomOccupancyReportViewBO
    {
        public string ReportTitle { get; set; }
        public string HeadName { get; set; }
        public int? HeadValue { get; set; }
        public string RoomType { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
