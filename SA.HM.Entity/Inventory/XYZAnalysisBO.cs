using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class XYZAnalysisBO
    {
        public string Name { get; set; }
        public decimal D1 { get; set; }
        public decimal D2 { get; set; }
        public decimal D3 { get; set; }
        public decimal D4 { get; set; }
        public decimal AnnualDemand { get; set; }
        public decimal AverageDemand { get; set; }
        public decimal STD { get; set; }
        public decimal CV { get; set; }
        public string Status { get; set; }
    }
}
