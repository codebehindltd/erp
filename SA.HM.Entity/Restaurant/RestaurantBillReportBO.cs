using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class RestaurantBillReportBO
    {
        public Nullable<int> BillId { get; set; }
        public string BillDate { get; set; }
        public string BillNumber { get; set; }
        public string SourceName { get; set; }
        public string TableNumber { get; set; }
        public Nullable<int> PaxQuantity { get; set; }
        public int CostCenterId { get; set; }
        public string CostCenter { get; set; }
        public string CustomerName { get; set; }
        public string PayMode { get; set; }
        public Nullable<int> BankId { get; set; }
        public string CardNumber { get; set; }
        public Nullable<decimal> TotalSales { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<decimal> DiscountedAmount { get; set; }
        public Nullable<decimal> NetAmount { get; set; }
        public Nullable<decimal> ServiceCharge { get; set; }
        public Nullable<decimal> VatAmount { get; set; }
        public Nullable<decimal> CitySDCharge { get; set; }
        public Nullable<decimal> AdditionalCharge { get; set; }
        public string AdditionalChargeType { get; set; }
        public Nullable<decimal> GrandTotal { get; set; }
        public Nullable<decimal> GuestTotalPaymentAmount { get; set; }
        public Nullable<decimal> GuestTotalRefundAmount { get; set; }
        public Nullable<int> KotDetailId { get; set; }
        public Nullable<int> KotId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string ItemType { get; set; }
        public string UnitHead { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string Category { get; set; }
        public Nullable<decimal> ItemUnit { get; set; }
        public Nullable<decimal> UnitRate { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string UserName { get; set; }
        public string WaiterName { get; set; }
        public Nullable<int> IsInclusiveBill { get; set; }
        public Nullable<int> IsVatServiceChargeEnable { get; set; }
        public string PaymentInformation { get; set; }
        public string TransactionType { get; set; }
        public string RestaurantVatString { get; set; }
        public string RestaurantServiceChargeString { get; set; }
        public string DiscountTitle { get; set; }
        public string MultipleTableAddedNumbers { get; set; }
        public string ClassificationWiseDiscount { get; set; }
        public string RestaurantSDCharge { get; set; }
        public string RestaurantAdditionalCharge { get; set; }
        public string Remarks { get; set; }
    }
}
