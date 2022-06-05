using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class RFQuotationFeedbackBO
    {
        public int RFQSupplierId { get; set; }
        public Nullable<int> TotalItemQuoted { get; set; }
        public Nullable<decimal> QuotedAmount { get; set; }
        public Nullable<decimal> ApplicableVatAit { get; set; }
        public Nullable<decimal> DeliveryCost { get; set; }
        public Nullable<decimal> TotalBillingAmount { get; set; }
        public string AdditionalInformation { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> SupplierId { get; set; }

        public Nullable<int> RFQId { get; set; }

        public List<RFQuotationItemsFeedbackBO> RFQuotationItemsFeedback = new List<RFQuotationItemsFeedbackBO>();
    }
}
