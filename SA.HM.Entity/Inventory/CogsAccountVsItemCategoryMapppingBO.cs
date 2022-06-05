using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class CogsAccountVsItemCategoryMapppingBO
    {
        public int CogsAccountMapId { get; set; }
        public int CategoryId { get; set; }
        public int NodeId { get; set; }
        public string Remarks { get; set; }

        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }

    }
}
