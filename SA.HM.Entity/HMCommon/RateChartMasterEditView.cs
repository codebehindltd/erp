using HotelManagement.Entity.HMCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class RateChartMasterEditView
    {
        public RateChartMaster RateChartMaster { get; set; }
        public List<RateChartDetail> RateChartDetails { get; set; }
    }
}
