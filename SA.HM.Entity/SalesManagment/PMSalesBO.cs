using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesManagment
{
   public class PMSalesBO
    {
        public int SalesId { get; set; }
        public int FieldId { get; set; }
        public int InvoiceId { get; set; }
        public string BillNumber { get; set; }
        public string Frequency { get; set; }
        public DateTime SalesDate { get; set; }
        public DateTime BillExpireDate { get; set; }
        public string InvoiceNumber { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public decimal SalesAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal GrandTotal { get; set; } 
        public string Remarks { get; set; }
        public int SiteInfoId { get; set; }
        public int BillingInfoId { get; set; }
        public int TechnicalInfoId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
    }
}
