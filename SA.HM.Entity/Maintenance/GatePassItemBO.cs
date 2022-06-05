using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Maintenance
{
    public class GatePassItemBO
    {
        public long GatePassItemId { get; set; }
        public long GatePassId { get; set; }
        public int CostCenterId { get; set; }
        public int ItemId { get; set; }
        public int StockById { get; set; }
        public decimal Quantity { get; set; }
        public string Description { get; set; }
        public int ReturnType { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Status { get; set; }
        public decimal? ApprovedQuantity { get; set; }

        //View Purpose
        public string CostCenter { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string StockBy { get; set; }
        public string ReturnTypeName { get; set; }
    }
}
