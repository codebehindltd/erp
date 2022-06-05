using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class SupplierNCompanyInfoForPurchaseInvoiceBO
    {
        public int POrderId { get; set; }
        public string PONumber { get; set; }
        public DateTime PODate { get; set; }
        public int CompanyId { get; set; }
        public string PODateString { get; set; }
        public DateTime ReceivedByDate { get; set; }
        public string ReceivedByDateString { get; set; }
        public int SupplierId { get; set; }
        public string Remarks { get; set; }
        public string DeliveryAddress { get; set; }
        public string ItemRemarks { get; set; }
        public int ProductId { get; set; }
        public string ItemName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string HeadName { get; set; }
        public string CreatedByName { get; set; }
        public string CheckedByName { get; set; }
        public string ApprovedByName { get; set; }
        public string CurrencyName { get; set; }
    }
}
