using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagement.Presentation.Website.Common.SDCTool
{
    public class SdcBillReportBO
    {
        public int? ItemId { get; set; }
        public string ItemCode { get; set; }
        public string HsCode { get; set; }
        public string ItemName { get; set; }
        public decimal ?UnitRate { get; set; }
        public int? PaxQuantity { get; set; }
        public string SdCategory { get; set; }
        public string VatCategory { get; set; }
    }
}