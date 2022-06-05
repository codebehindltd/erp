using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Membership
{
    public class MemberPaymentLedgerVwBo : PMMemberPaymentLedgerBO
    {
        public string Narration { get; set; }
        public decimal? ClosingBalance { get; set; }
        public decimal? BalanceCommulative { get; set; }
        public string ContactName { get; set; }
        public string MemberBillNumber { get; set; }
        public long MemberBillDetailsId { get; set; }

        //public int BillId { get; set; }

        public DateTime BillDate { get; set; }
    }
}
