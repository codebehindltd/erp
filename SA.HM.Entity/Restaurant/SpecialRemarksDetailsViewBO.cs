using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Inventory;

namespace HotelManagement.Entity.Restaurant
{
    public class SpecialRemarksDetailsViewBO
    {
        public List<InvItemSpecialRemarksBO> ItemSpecialRemarks { get; set; }
        public List<RestaurantKotSpecialRemarksDetailBO> KotRemarks { get; set; }
    }
}
