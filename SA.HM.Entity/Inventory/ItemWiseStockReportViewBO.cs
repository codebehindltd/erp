using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class ItemWiseStockReportViewBO
    {
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string StyleName { get; set; }
        public int? CostCenterId { get; set; }
        public string CostCenter { get; set; }
        public int? LocationId { get; set; }
        public string LocationName { get; set; }
        public decimal StockQuantity { get; set; }
        public int StockById { get; set; }
        public decimal? AverageCost { get; set; }
        public string HeadName { get; set; }
        public decimal ActualUsage { get; set; }
        public decimal UsageCost { get; set; }
        public decimal UnitPrice { get; set; }
        public string SerialNumber { get; set; }
        public string SerialStatus { get; set; }
        public string Code { get; set; }
        public string ManufacturerName { get; set; }
        public string Model { get; set; }
        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
    }
}
