using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class ReservationBillTransactionReportBO
    {
        public string PaymentDate { get; set; }
        public string ReservationNumber { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentType { get; set; }
        public string PaymentDescription { get; set; }
        public string POSTerminalBank { get; set; }
        public decimal? ReceivedAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public string OperatedBy { get; set; }
    }
}
