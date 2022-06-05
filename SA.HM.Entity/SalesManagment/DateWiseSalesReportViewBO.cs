using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesManagment
{
    public class DateWiseSalesReportViewBO
    {
        public int SalesId { get; set; }
        public string SalesDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public decimal SalesAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public int DetailId { get; set; }
        public string ServiceType { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemUnit { get; set; }
        public decimal UnitPriceLocal { get; set; }
        public string FieldValue1 { get; set; }       
        public decimal UnitPriceUsd { get; set; }
        public string FieldValue2 { get; set; }       
    }
}
