using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class PMProductReturnDetailsBO
    {
        public Int64 BillId { get; set; }
        public Int64 KotDetailId { get; set; }
        public long ReturnDetailsId { get; set; }
        public long ReturnId { get; set; }
        public int StockById { get; set; }
        public string StockBy { get; set; }
        public int ItemId { get; set; }
        public decimal OrderQuantity { get; set; }
        public decimal Quantity { get; set; }
        public decimal ReturnQuantity { get; set; }
        public Nullable<decimal> AverageCost { get; set; }

        public string ItemName { get; set; }
        public string HeadName { get; set; }
        public string ProductType { get; set; }
        public int OutId { get; set; }
        public int OutDetailsId { get; set; }
        public int FromCostCenterId { get; set; }
        public int CostCenterIdFrom { get; set; }
        public int FromLocationId { get; set; }
        public int LocationIdFrom { get; set; }
        public int ToCostCenterId { get; set; }
        public string IssueType { get; set; }
        public string CostCenter { get; set; }
    }
}
