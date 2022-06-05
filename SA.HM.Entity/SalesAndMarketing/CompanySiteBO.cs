using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class CompanySiteBO
    {
        public int SiteId { get; set; }
        public int CompanyId { get; set; }
        public string SiteName { get; set; }
        public string BusinessContactName { get; set; }
        public string BusinessContactEmail { get; set; }
        public string BusinessContactPhone { get; set; }
        public string TechnicalContactName { get; set; }
        public string TechnicalContactEmail { get; set; }
        public string TechnicalContactPhone { get; set; }
        public string BillingContactName { get; set; }
        public string BillingContactEmail { get; set; }
        public string BillingContactPhone { get; set; }
        public string Remarks { get; set; }

        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
    }
}
