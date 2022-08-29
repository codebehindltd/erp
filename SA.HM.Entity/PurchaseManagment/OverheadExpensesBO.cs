using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class OverheadExpensesBO
    {
        public int FinishedProductDetailsId { get; set; }
        public int FinishProductId { get; set; }
        public long ReceivedId { get; set; }
        public int NodeId { get; set; }
        public string AccountHead { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public int CostCenterId { get; set; }
    }
}
