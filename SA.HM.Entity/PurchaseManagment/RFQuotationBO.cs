using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class RFQuotationBO
    {
        public int RFQId { get; set; }
        public Nullable<int> StoreID { get; set; }
        public string Description { get; set; }
        public string IndentName { get; set; }
        public string PaymentTerm { get; set; }
        public Nullable<decimal> CreditDays { get; set; }
        public string DeliveryTerms { get; set; }
        public string SiteAddress { get; set; }
        public Nullable<System.DateTime> ExpireDateTime { get; set; }
        public Nullable<decimal> VAT { get; set; }
        public Nullable<decimal> AIT { get; set; }
        public string IndentPurpose { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string IndentBy { get; set; }



        public List<RFQuotationItemBO> RFQuotationItems = new List<RFQuotationItemBO>();
    }
}
