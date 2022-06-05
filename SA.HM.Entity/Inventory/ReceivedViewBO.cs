using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class ReceivedViewBO
    {
        public PMProductReceivedBO Received { get; set; }
        public List<PMProductReceivedDetailsBO> ReceivedDetails { get; set; }

        public int POrderId { get; set; }
        public int CostCenterId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }

        public string ReceivedProductGrid { get; set; }
    }
}
