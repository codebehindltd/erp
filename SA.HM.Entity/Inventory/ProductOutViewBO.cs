using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.PurchaseManagment;

namespace HotelManagement.Entity.Inventory
{
    public class ProductOutViewBO
    {
        public PMProductOutBO ProductOut { get; set; }
        public List<PMProductOutDetailsBO> ProductOutDetails { get; set; }

        public PMSupplierProductReturnBO SupplierProductReturn { get; set; }
        public List<PMProductReturnDetailsBO> SupplierProductReturnDetails { get; set; }
    }
}
