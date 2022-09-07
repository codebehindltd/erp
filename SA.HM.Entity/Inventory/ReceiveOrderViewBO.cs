using HotelManagement.Entity.PurchaseManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class ReceiveOrderViewBO
    {
        public PMProductReceivedBO ProductReceived { get; set; }
        public List<PMProductReceivedDetailsBO> ProductReceivedDetails { get; set; }
        public List<PMProductReceivedDetailsBO> ProductReceivedDetailsSummary { get; set; }
        public List<PMProductSerialInfoBO> ProductSerialInfo { get; set; }
        public List<OverheadExpensesBO> OverheadExpenseInfoList { get; set; }
        public List<OverheadExpensesBO> PaymentInformationList { get; set; }

        public string PurchaseOrderGrid { get; set; }
        public int CostCenterId { get; set; }
    }
}
