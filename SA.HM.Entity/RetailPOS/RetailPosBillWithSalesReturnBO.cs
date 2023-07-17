using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.RetailPOS
{
    public class RetailPosBillWithSalesReturnBO
    {
        public int BillId { get; set; }
        public string BillNumber { get; set; }
        public string TransactionType { get; set; }
        public Nullable<int> ReturnBillId { get; set; }
        public Nullable<System.DateTime> BillDate { get; set; }
        public string BillDateDisplay { get; set; }
        public string CostCenter { get; set; }
        public string CashierName { get; set; }
        public string CustomerName { get; set; }
        public Nullable<decimal> SalesAmount { get; set; }
        public string DiscountType { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<decimal> CalculatedDiscountAmount { get; set; }
        public Nullable<decimal> AmountAfterDiscount { get; set; }
        public Nullable<decimal> InvoiceVatAmount { get; set; }
        public Nullable<decimal> InvoiceServiceCharge { get; set; }
        public Nullable<decimal> InvoiceAdditionalCharge { get; set; }
        public Nullable<decimal> GrandTotal { get; set; }
        public Nullable<decimal> RoundedAmount { get; set; }
        public Nullable<decimal> RoundedGrandTotal { get; set; }
        public Nullable<bool> IsInvoiceVatAmountEnable { get; set; }
        public Nullable<bool> IsInvoiceServiceChargeEnable { get; set; }
        public Nullable<decimal> InvoiceCitySDCharge { get; set; }
        public int KotId { get; set; }
        public Nullable<int> BearerId { get; set; }
        public Nullable<int> ReferenceKotId { get; set; }
        public Nullable<decimal> BagWaight { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> UnitRate { get; set; }
        public Nullable<decimal> ItemUnit { get; set; }
        public string UnitHead { get; set; }
        public Nullable<decimal> ItemTotalAmount { get; set; }
        public Nullable<decimal> PointsRedeemedAmount { get; set; }
        public Nullable<decimal> PointsAwarded { get; set; }
        public Nullable<decimal> PointsRedeemed { get; set; }
        public Nullable<decimal> BalancePoints { get; set; }
        public string Attention { get; set; }
        public string PaymentInstruction { get; set; }
        public string BillSubject { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public string IssueDateDisplay { get; set; }
        public Nullable<System.DateTime> FlightDate { get; set; }
        public string FlightDateDisplay { get; set; }
        public string TicketNumber { get; set; }
        public string RoutePath { get; set; }
        public string AirlineName { get; set; }
        public string Remarks { get; set; }
        public string BillDescription { get; set; }
        public Nullable<decimal> CompanyPreviousDue { get; set; }
        public Nullable<decimal> CompanyBillDueTotal { get; set; }
    }
}
