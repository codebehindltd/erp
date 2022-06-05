using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class RFQuotationItemsFeedbackBO
    {

        public int RFQSupplierItemId { get; set; }
        public Nullable<int> RFQSupplierId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }

        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> OfferedUnitPrice { get; set; }
        public Nullable<decimal> OfferedUnitPriceWithVatAit { get; set; }
        public Nullable<decimal> BillingAmount { get; set; }

        public Nullable<DateTime> QuoteDate { get; set; }
        public Nullable<decimal> AdvanceAmount { get; set; }
        public Nullable<int> OfferValidation { get; set; }
        public Nullable<int> StockUnit { get; set; }
        public Nullable<int> DeliveryDuration { get; set; }

        public Nullable<int> RFQItemId { get; set; }

        public string ItemName { get; set; }
        public string ItemRemarks { get; set; }
        public string StockUnitName { get; set; }
        public string SupplierName { get; set; }

        public List<RFQuotationItemDetailsFeedbackBO> RFQuotationItemSpecificationsFeedback = new List<RFQuotationItemDetailsFeedbackBO>();

    }
}
