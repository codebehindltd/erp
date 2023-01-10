using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class NRVViewBO
    {
        public InvNutrientInfoBO NRVMasterInfo { get; set; }
        public List<InvNutrientInfoBO> NRVDetails { get; set; }
    }
}
