using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class IssueForBO
    {

        public long Id { get; set; }
        public string IssueName { get; set; }
        public int DefaultStockLocationId { get; set; }
    }
}
