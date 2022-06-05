using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class PMProductReturnDetailsBO
    {
        public int ReturnDetailsId { get; set; }
        public int ReturnId { get; set; }
        public int? CostCenterIdFrom { get; set; }
        public int? LocationIdFrom { get; set; }
        public int? CostCenterIdTo { get; set; }
        public int? LocationIdTo { get; set; }
        public int? StockById { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal? AverageCost { get; set; }

        public string ProductName { get; set; }
        public string StockBy { get; set; }
        public string CostCenter { get; set; }
        public string Location { get; set; }
    }
}
