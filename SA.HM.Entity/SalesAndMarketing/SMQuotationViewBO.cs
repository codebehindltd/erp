using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMQuotationViewBO: SMQuotationBO
    {
        public string ProposalDateToDisplay { get; set; }
        public string CompanyName { get; set; }       
        public string ServiceName { get; set; }
        public string LocationName { get; set; }
        public string ContractPeriodName { get; set; }
        public string BillingPeriodName { get; set; }
        public string ItemServiceDeliveryName { get; set; }
        public string CurrentVendorName { get; set; }
        public string DealName { get; set; }
        public bool HasItem { get; set; }
    }
}
