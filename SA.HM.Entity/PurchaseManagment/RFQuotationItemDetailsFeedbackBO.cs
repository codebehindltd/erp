using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class RFQuotationItemDetailsFeedbackBO
    {
        public int FeedbackId { get; set; }
        public Nullable<int> RFQSupplierId { get; set; }
        public Nullable<int> RFQSupplierItemId { get; set; }
        public Nullable<int> RFQuotationItemDetailsId { get; set; }
        public string Feedback { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
    }
}
