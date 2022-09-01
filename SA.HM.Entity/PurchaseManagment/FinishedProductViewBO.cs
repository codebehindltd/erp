using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class FinishedProductViewBO
    {
        public FinishedProductBO FinishedProduct { get; set; }
        public List<FinishedProductDetailsBO> FinisProductDetails { get; set; }
        public List<FinishedProductDetailsBO> FinisProductRMDetails { get; set; }
        public List<OverheadExpensesBO> FinishProductOEDetails { get; set; }
    }
}
