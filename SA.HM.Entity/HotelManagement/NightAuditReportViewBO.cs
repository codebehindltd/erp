using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class NightAuditReportViewBO
    {
        public string ReportForDate { get; set; }
        public string RegistrationNumber { get; set; }
        public string BillNumber { get; set; }
        public string ServiceName { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? VatAmount { get; set; }
        public decimal? Charge { get; set; }
        public decimal? CitySDCharge { get; set; }
        public decimal? AdditionalCharge { get; set; }
        public int? TotalGuest { get; set; }
    }
}
