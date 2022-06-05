using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMQuotationWiseSalesBO
    {

        public long QuotationId { get; set; }
        public decimal DealAmount { get; set; }

        public string DealName { get; set; }
        public string CompanyName { get; set; }

        public string CompanyAddress { get; set; }
        public string EmailAddress { get; set; }
        public string WebAddress { get; set; }
        public string ContactPerson { get; set; }

        public string ContactDesignation { get; set; }
        public string ContactNumber { get; set; }

        public string QuotationNo { get; set; }

    }
}
