using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class PMProductOutDetailsBO
    {
        public int OutDetailsId { get; set; }
        public int OutId { get; set; }
        public string OutDateString { get; set; }
        public Int32 CostCenterIdFrom { get; set; }
        public Int32 LocationIdFrom { get; set; }
        public Int32? CostCenterIdTo { get; set; }
        public Int32? LocationIdTo { get; set; }
        public int StockById { get; set; }
        public int ProductId { get; set; }
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
        public decimal AverageCost { get; set; }
        public int CategoryId { get; set; }
        public int SerialId { get; set; }
        public string SerialNumber { get; set; }
        public string IssueNumber { get; set; }
        public string ProductName { get; set; }
        public string ItemName { get; set; }
        public string ProductType { get; set; }
        public string CostCenterFrom { get; set; }
        public string CostCenterTo { get; set; }
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public string StockBy { get; set; }
        public string CategoryName { get; set; }
        public string Code { get; set; }
        public string UserName { get; set; }
        public string ReferenceNumberText { get; set; }
        public string ReferenceNumber { get; set; }
        public string ReferenceDate { get; set; }
        public string ReferenceBy { get; set; }
        public int? AdjustmentStockById { get; set; }
        public decimal? AdjustmentQuantity { get; set; }
        public String ApprovalStatus { get; set; }
        public Decimal? ApprovedQuantity { get; set; }
        public Decimal? DeliveredQuantity { get; set; }
        public Decimal StockQuantity { get; set; }
        public Decimal RemainingTransferQuantity { get; set; }
        public Int64? RequisitionDetailsId { get; set; }

        public string CheckedByName { get; set; }
        public string ApprovedByName { get; set; }
        public string TransferedByName { get; set; }
        public int FromCompanyId { get; set; }
        public string FromCompany { get; set; }
        public int FromProjectId { get; set; }
        public string FromProject { get; set; }
        public int ToCompanyId { get; set; }
        public string ToCompany { get; set; }
        public int ToProjectId { get; set; }
        public string ToProject { get; set; }

        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int StyleId { get; set; }
        public string ColorText { get; set; }
        public string SizeText { get; set; }
        public string StyleText { get; set; }
    }
}
