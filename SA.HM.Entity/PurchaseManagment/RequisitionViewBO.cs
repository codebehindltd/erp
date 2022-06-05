using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class RequisitionViewBO
    {
        public PMRequisitionBO Requisition { get; set; }
        public List<PMRequisitionDetailsBO> RequisitionDetails { get; set; }
    }
}
