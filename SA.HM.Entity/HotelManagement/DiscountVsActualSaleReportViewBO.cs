using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class DiscountVsActualSaleReportViewBO
    {
        public string ReportTitle { get; set; }
        public string AmountType { get; set; }
        public string MonthName { get; set; }
        public int? MonthValue { get; set; }
        public decimal? Amount { get; set; }
    }
}
