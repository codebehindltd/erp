using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class RFQuotationItemDetailBO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public Nullable<int> RFQItemId { get; set; }
    }
}
