using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class RFQuotationItemBO
    {


        public string StockUnitName { get; set; }
        public string ItemName { get; set; }
        public string CreatedByName { get; set; }
        public int RFQItemId { get; set; }
        public Nullable<int> RFQId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public Nullable<int> StockUnit { get; set; }
        public Nullable<decimal> Quantity { get; set; }

        public string IndentName { get; set; }
        public string PaymentTerm { get; set; }
        public Nullable<decimal> CreditDays { get; set; }
        public string DeliveryTerms { get; set; }
        public string SiteAddress { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }

        public List<RFQuotationItemDetailBO> RFQuotationItemSpecifications = new List<RFQuotationItemDetailBO>();
    }
}
