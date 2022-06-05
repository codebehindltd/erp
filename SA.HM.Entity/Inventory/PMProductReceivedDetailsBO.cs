using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class PMProductReceivedDetailsBO
    {
        public int ReceiveDetailsId { get; set; }
        public int ReceivedId { get; set; }
        public string ReceiveNumber { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string StringReceivedDate { get; set; }
        public int ProductId { get; set; }
        public int ItemId { get; set; }
        public int ColorId { get; set; }
        public string ColorText { get; set; }
        public int SizeId { get; set; }
        public string SizeText { get; set; }
        public int StyleId { get; set; }
        public string StyleText { get; set; }
        public int CostCenterId { get; set; }
        public int StockById { get; set; }
        public int LocationId { get; set; }
        public decimal Quantity { get; set; }
        public int BagQuantity { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal ReturnQuantity { get; set; }
        public decimal PurchaseQuantity { get; set; }
        public string SerialNumber { get; set; }
        public int SupplierId { get; set; }
        public int CompanyId { get; set; }
        public int POrderId { get; set; }
        public string PONumber { get; set; }
        public string StringPODate { get; set; }
        public string POByName { get; set; }
        public string ItemName { get; set; }
        public string ProductType { get; set; }
        public string CostCenter { get; set; }
        public string StockBy { get; set; }
        public string LocationName { get; set; }
        public string SupplierName { get; set; }
        public decimal OrderedQuantity { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal PurchasePrice { get; set; }
        public string Status { get; set; }
        public string ReceivedStatus { get; set; }
        public decimal RemainingQuantity { get; set; }
        public decimal StockQuantity { get; set; }
        public string DefaultStockBy { get; set; }
        public decimal ApprovedReceiveQuantity { get; set; }
        public decimal RemainingReceiveQuantity { get; set; }
        public string ReceivedByName { get; set; } 
        public string CheckedByName { get; set; }
        public string ApprovedByName { get; set; }
        public string Remarks { get; set; }
    }
}
