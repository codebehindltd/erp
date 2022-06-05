using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class PMProductReceivedReportViewBO
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal AverageCost { get; set; }
        public string ReceivedDate { get; set; }
        public string UserName { get; set; }
        public string PONumber { get; set; }
        public string SerialNumber { get; set; }
        public string UnitHead { get; set; }
        public string CostCenterFrom { get; set; }
        public string CostCenterTo { get; set; }
        public string LocationNameFrom { get; set; }
        public string LocationNameTo { get; set; }
        public int? CostCenterIdFrom { get; set; }
        public int? CostCenterIdTo { get; set; }
        public string Category { get; set; }
        public string Remarks { get; set; }
        public string IssueType { get; set; }
    }
}
