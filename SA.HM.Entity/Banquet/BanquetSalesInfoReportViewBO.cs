using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Banquet
{
    public class BanquetSalesInfoReportViewBO
    {
        public DateTime ServiceDate { get; set; }
        public string ServiceDisplayDate { get; set; }
        public string ReferenceNo { get; set; }
        //public Nullable<int> ServiceId { get; set; }
        //public string CategoryName { get; set; }
        //public string ServiceName { get; set; }
        //public string RoomNumber { get; set; }
        //public Nullable<decimal> ItemQuantity { get; set; }
        //public Nullable<decimal> ServiceRate { get; set; }
        //public Nullable<decimal> DiscountAmount { get; set; }
        //public Nullable<decimal> VatAmount { get; set; }
        //public Nullable<decimal> ServiceCharge { get; set; }
        //public string SalesType { get; set; }
        //public Nullable<int> IsDiscountHead { get; set; }

        public Nullable<int> CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string RoomNumber { get; set; }
        public Nullable<decimal> ItemQuantity { get; set; }
        public Nullable<decimal> ServiceRate { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }        
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
        public Nullable<decimal> UnitRate { get; set; }
    }
}
