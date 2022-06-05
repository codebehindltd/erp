using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class SalesTransactionReportViewBO
    {
        public string ServiceDate { get; set; }
        public string RoomNumber { get; set; }
        public string BillNumber { get; set; }
        public string PaymentDescription { get; set; }
        public string PaymentMode { get; set; }
        public string POSTerminalBank { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public string OperatedBy { get; set; }
    }
}
