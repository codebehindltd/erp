using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class BillInfoViewBO
    {
        public int TransactionId { get; set; }
        public string TransactionName { get; set; }

        public decimal TotalSales { get; set; }
        public string BillNumber { get; set; }
        public string ServiceDate { get; set; }
    }
}
