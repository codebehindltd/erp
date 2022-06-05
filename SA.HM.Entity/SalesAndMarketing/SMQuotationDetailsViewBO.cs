using HotelManagement.Entity.HotelManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMQuotationDetailsViewBO
    {
        public GuestCompanyBO Company { get; set; }
        public SMContactInformationViewBO Contact { get; set; }
        public SMQuotationViewBO Quotation { get; set; }
        public List<SMQuotationDetailsBO> QuotationItemDetails { get; set; }
        public List<SMQuotationDetailsBO> QuotationServiceDetails { get; set; }
        public List<SMQuotationDetailsBO> QuotationDetails { get; set; }

        public SMQuotationDetailsViewBO()
        {
            QuotationItemDetails = new List<SMQuotationDetailsBO>();
            QuotationServiceDetails = new List<SMQuotationDetailsBO>();
            QuotationDetails = new List<SMQuotationDetailsBO>();
        }
    }
}
