using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvItemSuppierMappingBO
    {
        public int MappingId { get; set; }
        public int ItemId { get; set; }
        public int SupplierId { get; set; }
    }
}
