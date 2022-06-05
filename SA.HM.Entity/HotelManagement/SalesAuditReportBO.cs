using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class SalesAuditReportBO
    {
        public string TransactionDate { get; set; }
        public decimal Cash { get; set; }
        public decimal Credit { get; set; }
        public decimal TotalCashCredit { get; set; }
        public decimal TotalRoomSales { get; set; }

        public decimal PaymentAmount { get; set; }
        public int NodeId { get; set; }
        public string NodeHead { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
    }
}
