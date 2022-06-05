using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesManagment
{
    public class PMSalesBillPreviewBO
    {
        public int SalesId { get; set; }
        public string BillNumber { get; set; }
        public DateTime SalesDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public decimal SalesAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public int FieldId { get; set; }
        public string Frequency { get; set; }
        public DateTime BillExpireDate { get; set; }
        public decimal DueAmount { get; set; }
        public int DetailId { get; set; }
        public string ServiceType { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal BillAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
