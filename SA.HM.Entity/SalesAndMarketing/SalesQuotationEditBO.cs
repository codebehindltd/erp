using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SalesQuotationEditBO
    {
        public SMQuotationBO Quotation { get; set; }
        public List<SMQuotationDetailsBO> QuotationItemDetails { get; set; }
        public List<SMQuotationDetailsBO> QuotationServiceDetails { get; set; }
        public List<SMQuotationDetailsBO> QuotationDetails { get; set; }
    }
}
