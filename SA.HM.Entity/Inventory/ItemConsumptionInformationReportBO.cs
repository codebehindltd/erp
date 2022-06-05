using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class ItemConsumptionInformationReportBO
    {
        public string Consumer { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string ReferenceNumber { get; set; }
        public string Type { get; set; }
        public string CostCenterFrom { get; set; }
        public string UnitHead { get; set; }
        public decimal Quantity { get; set; }
        public string SerialNumber { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Category { get; set; }
    }
}
