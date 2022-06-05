using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvItemConsumptionAdjustmentViewBO
    {
        public List<PMProductOutDetailsBO> ProductOutDetails { get; set; }
        public List<InvUnitHeadBO> AllUnitHeads { get; set; }
    }
}
