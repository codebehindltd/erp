using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class InvManufacturerBO
    {
        public Int64 ManufacturerId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
    }
}
