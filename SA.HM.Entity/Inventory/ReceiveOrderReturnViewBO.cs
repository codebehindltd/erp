using HotelManagement.Entity.PurchaseManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class ReceiveOrderReturnViewBO
    {
        public PMSupplierProductReturnBO ProductReturn { get; set; }
        public List<PMSupplierProductReturnDetailsBO> ProductReturnDetails { get; set; }
        public List<PMSupplierProductReturnSerialBO> ProductReturnSerialInfo { get; set; }
        public List<PMProductReceivedBO> ProductReceive = new List<PMProductReceivedBO>();
    }
}
