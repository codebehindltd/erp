using HotelManagement.Entity.HotelManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMQuotationReportViewBO
    {
        public List<GuestCompanyBO> Company { get; set; }
        public List<SMQuotationViewBO> Quotation { get; set; }
        public List<SMQuotationDetailsBO> QuotationItemDetails { get; set; }
        public List<SMQuotationDetailsBO> QuotationServiceDetails { get; set; }
        public List<SMQuotationDiscountDetails> QuotationRestaurantDetails { get; set; }
        public List<SMQuotationDiscountDetails> QuotationBanquetDetails { get; set; }
        public List<SMQuotationDiscountDetails> QuotationGuestRoomDetails { get; set; }
        public List<SMQuotationDiscountDetails> QuotationServiceOutletDetails { get; set; }
        public SMQuotationReportViewBO()
        {
            Company = new List<GuestCompanyBO>();
            Quotation = new List<SMQuotationViewBO>();
            QuotationItemDetails = new List<SMQuotationDetailsBO>();
            QuotationServiceDetails = new List<SMQuotationDetailsBO>();
            QuotationRestaurantDetails = new List<SMQuotationDiscountDetails>();
            QuotationBanquetDetails = new List<SMQuotationDiscountDetails>();
            QuotationGuestRoomDetails = new List<SMQuotationDiscountDetails>();
            QuotationServiceOutletDetails = new List<SMQuotationDiscountDetails>();
        }
    }
}
