using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvCategoryCostCenterMappingBO
    {
        public int MappingId { get; set; }
        public int CostCenterId { get; set; }
        public int CategoryId { get; set; }
    }
}
