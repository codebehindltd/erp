using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class PMPurchaseOrderInfoReportBO
    {
        public string CategoryName { get; set; }
        public string CategoryCode { get; set; }
        public string ServiceType { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string PONumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int DetailId { get; set; }
        public int POrderId { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityReceived { get; set; }
        public int ProductId { get; set; }
        public string ProductTransactionId { get; set; }
    }
}
