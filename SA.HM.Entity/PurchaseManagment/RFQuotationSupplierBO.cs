using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class RFQuotationSupplierBO
    {
        public int Id { get; set; }
        public Nullable<int> SupplierId { get; set; }
        public Nullable<int> RFQId { get; set; }
    }
}
