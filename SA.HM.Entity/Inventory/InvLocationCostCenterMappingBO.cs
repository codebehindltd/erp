using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvLocationCostCenterMappingBO
    {
        public int MappingId { get; set; }
        public int CostCenterId { get; set; }
        public int LocationId { get; set; }
    }
}
