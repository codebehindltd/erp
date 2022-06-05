
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestServiceBillPaymentDetailsViewBO
    {
        public GHServiceBillBO ServiceBill { get; set; }
        public List<GuestBillPaymentBO> ServiceBillDetails { get; set; }
    }
}
