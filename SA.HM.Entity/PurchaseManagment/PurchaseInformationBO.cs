
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class PurchaseInformationBO
    {
        public int POrderId { get; set; }
        public string PODate { get; set; }
        public DateTime? ReceivedByDate { get; set; }
        public string PONumber { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public string ApprovedStatus { get; set; }
        public string Remarks { get; set; }
        public int? DetailId { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductCategory { get; set; }
        public string CategoryCode { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? Quantity { get; set; }
        public decimal QuantityReceived { get; set; }
        public string MessureUnit { get; set; }
        public string CostCenter { get; set; }


    }
}
