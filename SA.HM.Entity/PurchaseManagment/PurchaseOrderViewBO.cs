using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class PurchaseOrderViewBO
    {
        public PMPurchaseOrderBO PurchaseOrder { get; set; }
        public List<PMPurchaseOrderDetailsBO> PurchaseOrderDetails { get; set; }

        public List<PMPurchaseOrderDetailsBO> PurchaseOrderDetailsSummary { get; set; }

        public string PurchaseOrderGrid { get; set; }
        public int CostCenterId { get; set; }
    }
}
