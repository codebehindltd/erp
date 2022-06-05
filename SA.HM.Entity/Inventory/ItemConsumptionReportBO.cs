using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class ItemConsumptionReportBO
    {
        public string   CostCenterFrom      { get; set; }
        public string   LocationNameFrom    { get; set; }
        public string   Category            { get; set; }
        public string   ReceivedDate        { get; set; }
        public string   IssueNumber         { get; set; }
        public string   Code                { get; set; }
        public string   ProductOutFor       { get; set; }
        public string   ProductName         { get; set; }
        public string   UnitHead            { get; set; }
        public decimal  AverageCost         { get; set; }
        public decimal  Quantity            { get; set; }
        public string   SerialNumber        { get; set; }
        public string   UserName            { get; set; }
        public string   Remarks             { get; set; }
    }
}
