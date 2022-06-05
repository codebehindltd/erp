using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestPaymentReportViewBO
    {
        public string GuestType { get; set; }
        public string PaymentType { get; set; }
        public string PaymentMode { get; set; }
        public string BillOrRoomNumber { get; set; }
        public decimal? PaymentAmount { get; set; }
        public string PaymentDate { get; set; }
    }
}
