using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesManagment
{
    public class PMSalesInvoiceBO
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime BillFromDate { get; set; }
        public DateTime BillToDate { get; set; }
        public int SalesId { get; set; }
        public decimal InvoiceAmount { get; set; }
        public DateTime BillDueDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public decimal ReceiveAmount { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public int CustomerId { get; set; }
    }
}
