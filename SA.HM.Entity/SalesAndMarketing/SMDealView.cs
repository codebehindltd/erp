using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMDealView
    {
        public long DealId { get; set; }
        public string Owner { get; set; }        
        public string Name { get; set; }
        public decimal? Amount { get; set; }
        public string Stage { get; set; }
        public decimal Complete { get; set; }
        public string Company { get; set; }
        public int CompanyId { get; set; }
        public string Industry { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string LifeCycleStage { get; set; }
        public DateTime LastActivityDateTime { get; set; }
        public string ShippingStreet { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingCountry { get; set; }
        public string ShippingPostCode { get; set; }

        public string ImplementationFeedback { get; set; }
        public string ImplementationStatus { get; set; }
        public bool IsCloseWon { get; set; }

        public virtual List<SMDealWiseContactMap> Contacts { get; set; }
        public virtual List<SMDealWiseContactMap> CompanyContacts { get; set; }
        
        
    }
}
