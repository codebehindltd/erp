using HotelManagement.Entity.PurchaseManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class ProductionInfoViewBO
    {
        public FinishedProductBO FinishedProduct { get; set; }
        public List<FinishedProductDetailsBO> RMInformations { get; set; }
        public List<FinishedProductViewBO> FGInformations { get; set; }
        public List<OverheadExpensesBO> OEInformations { get; set; }
    }
}
