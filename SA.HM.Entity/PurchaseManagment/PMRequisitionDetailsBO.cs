using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class PMRequisitionDetailsBO
    {
        public long RequisitionDetailsId { get; set; }
        public int RequisitionId { get; set; }
        public int CategoryId { get; set; }
        public int ItemId { get; set; }

        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int StyleId { get; set; }
        public string ColorText { get; set; }
        public string SizeText { get; set; }
        public string StyleText { get; set; }

        public int StockById { get; set; }
        public decimal Quantity { get; set; }
        public decimal? ApprovedQuantity { get; set; }
        public decimal? DeliveredQuantity { get; set; }
        public string ApprovedStatus { get; set; }
        public string InitialApprovedStatus { get; set; }
        public string DeliverStatus { get; set; }
        public decimal StockQuantity { get; set; }
        public string ItemName { get; set; }
        public string ProductType { get; set; }
        public string ItemCode { get; set; }
        public string HeadName { get; set; }
        public string CategoryName { get; set; }
        public string RequisitionRemarks { get; set; }
        public string CheckedByName { get; set; }
        public string ApprovedByName { get; set; }
        public string ItemRemarks { get; set; }
        public int OutDetailsId { get; set; }
        public decimal RemainingTransferQuantity { get; set; }
        public decimal ApprovedTransferQuantity { get; set; }
        public decimal? AverageCost { get; set; }

        public decimal? CurrentStockFromStore { get; set; }
        public decimal? LastTransferQuantity { get; set; }
        public decimal? LastRequisitionQuantity { get; set; }
        public string RequisitionBy { get; set; }
        public string CheckedBy { get; set; }
        public string LastTransferType { get; set; }
    }
}
