using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMCostAnalysisView
    {
        public SMCostAnalysis CostAnalysis { get; set; }
        public List<SMCostAnalysisDetail> AllItems { get; set; }
        public List<SMCostAnalysisDetail> Items { get; set; }
        public List<SMCostAnalysisDetail> Services { get; set; }
    }
}
