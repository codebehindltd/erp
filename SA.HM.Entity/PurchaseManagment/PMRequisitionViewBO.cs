using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class PMRequisitionViewBO:PMRequisitionBO
    {
        //public bool IsCanEdit { get; set; }
        //public bool IsCanDelete { get; set; }
        public bool IsCanChecked { get; set; }
        public bool IsCanApproved { get; set; }
    }
}
