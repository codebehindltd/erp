using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SummaryGuestInfoViewBO
    {
        public String TransactionHead { get; set; }
        public DateTime TransactionDate { get; set; }
        public int StayedNights { get; set; }
        public String DisplayTransactionDate { get; set; }
    }
}
