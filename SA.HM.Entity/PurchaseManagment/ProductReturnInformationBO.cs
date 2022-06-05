using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class ProductReturnInformationBO
    {
        public long ReturnId { get; set; }
        public string ReturnDate { get; set; }
        public int SupplierId { get; set; }
        public string Supplier { get; set; }
        public string ReturnType { get; set; }
        public int? TransactionId { get; set; }
        public string InvoiceNumber { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string SerialNumber { get; set; }
        public decimal? Quantity { get; set; }
        public string Remarks { get; set; }
    }
}
