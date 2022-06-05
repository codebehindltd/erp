using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesManagment
{
   public class PMSalesBillingInfoBO
    {

        public int BillingInfoId { get; set; }
        public int SelectedBillingInfoId { get; set; }
        public int CustomerId { get; set; }
        public string BillingContactPerson { get; set; }
        public string BillingPersonDepartment { get; set; }
        public string BillingPersonDesignation { get; set; }
        public string BillingPersonPhone { get; set; }
        public string BillingPersonEmail { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

    }
}
