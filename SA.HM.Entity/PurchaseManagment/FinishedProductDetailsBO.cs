using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class FinishedProductDetailsBO
    {
        public int FinishedProductDetailsId { get; set; }
        public int FinishProductId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public int SizeId { get; set; }
        public string SizeName { get; set; }
        public int StyleId { get; set; }
        public string StyleName { get; set; }
        public int StockById { get; set; }
        public string UnitName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int BagQuantity { get; set; }
        public string StockBy { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string CostCenter { get; set; }
        public DateTime ProductionDate { get; set; }
        public string ProductionDateString { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public string Remarks { get; set; }
        public string ProductType { get; set; }
        public Int64 ProductDetailsId { get; set; }
        public Int64 ProductionId { get; set; }
        public decimal PercentageValue { get; set; }
    }
}
