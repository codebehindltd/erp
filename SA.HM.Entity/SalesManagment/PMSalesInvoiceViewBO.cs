using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesManagment
{
    public class PMSalesInvoiceViewBO
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string EmailAddress { get; set; }
        public string WebAddress { get; set; }
        public string ContactNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerEmailAddress { get; set; }
        public string CustomerWebAddress { get; set; }
        public string CustomerContactNumber { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string SwiftCode { get; set; }
        public string AccountName { get; set; }
        public string AccountNoUSD { get; set; }
        public string AccountNoLocal { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime BillFromDate { get; set; }
        public DateTime BillToDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string InvoiceFor { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal AdvanceOrDueAmount { get; set; }
        public int FieldId { get; set; }
        public string Currency { get; set; }
    }
}
