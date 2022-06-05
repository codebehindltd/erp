using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class PMPurchaseOrderDetailsBO
    {
        public int DetailId { get; set; }
        public int POrderId { get; set; }
        public int RequisitionId { get; set; }
        public int ProductId { get; set; }
        public int ItemId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int StyleId { get; set; }
        public string ColorText { get; set; }
        public string SizeText { get; set; }
        public string StyleText { get; set; }
        public string ItemName { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public string ProductType { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal ActualReceivedQuantity { get; set; }
        public string MessureUnit { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int CostCenterId { get; set; }
        public int StockById { get; set; }        
        public string StockBy { get; set; }
        public string ReceivedStatus { get; set; }
        public decimal StockQuantity { get; set; }
        public decimal RemainingQuantity { get; set; }
        public int DefaultStockLocationId { get; set; }
        public string Remarks { get; set; }
        public string PRNumber { get; set; }
        public decimal ApprovedQuantity { get; set; }
        public int RequisitionDetailsId { get; set; }
        public string OrderRemarks { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
        public decimal? LastPurchasePrice { get; set; }
        public int ReceivedId { get; set; }
        public int ReceiveDetailsId { get; set; }
        public int SupplierId { get; set; }
        public string OrderType { get; set; }
        public decimal ApprovedReceiveQuantity { get; set; }
        public decimal RemainingPOQuantity { get; set; }
        public decimal ApprovedPOQuantity { get; set; }
        public decimal RemainingReceiveQuantity { get; set; }
        public int CurrencyId { get; set; }
        public decimal ConvertionRate { get; set; }
        public long BillId { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal CashIncentive { get; set; }
    }
}
