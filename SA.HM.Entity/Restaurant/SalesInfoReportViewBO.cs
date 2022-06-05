using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class SalesInfoReportViewBO
    {
        public DateTime ServiceDate { get; set; }
        public string ServiceDisplayDate { get; set; }
        public string ReferenceNo { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string RoomNumber { get; set; }
        public Nullable<decimal> ItemQuantity { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<decimal> ServiceRate { get; set; }
        public Nullable<decimal> ServiceCharge { get; set; }
        public Nullable<decimal> CitySDCharge { get; set; }        
        public Nullable<decimal> VatAmount { get; set; }
        public Nullable<decimal> AdditionalCharge { get; set; }
        public Nullable<decimal> ItemCost { get; set; }
        public string SalesType { get; set; }
        public Nullable<int> IsDiscountHead { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<decimal> TotalSalesAmount { get; set; }
        public Nullable<decimal> ProfitNLossAmount { get; set; }
        public string BillNumber { get; set; }
        public DateTime BillDate { get; set; }
        public Nullable<int> UserInfoId { get; set; }
        public string UserName { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> UnitRate { get; set; }
        public Nullable<decimal> ItemUnit { get; set; }
        public Nullable<decimal> LineTotal { get; set; }
        public Nullable<decimal> GrandTotal { get; set; }
        public Nullable<decimal> RoundedAmount { get; set; }
        public Nullable<decimal> RoundedGrandTotal { get; set; }
        public Nullable<decimal> TotalServiceCharge { get; set; }
        public Nullable<decimal> TotalCitySDCharge { get; set; }
        public Nullable<decimal> TotalVat { get; set; }
        public Nullable<decimal> TotalAdditionalCharge { get; set; }
        public decimal? CalculatedDiscountAmount { get; set; }
        public string PaymentInformation { get; set; }
        public string GuestTotalPaymentInformation { get; set; }

        public string CashierName { get; set; }
        public decimal CQty { get; set; }
        public decimal CCost { get; set; }
        public string PaymentRemarks { get; set; }
        public int KotId { get; set; }
        public string KotDate { get; set; }
        public string KotTime { get; set; }
        public List<SalesInfoReportViewBO> salesInfoList { get; set; }
        public List<SalesInfoReportViewBO> salesInfoList2 { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}
