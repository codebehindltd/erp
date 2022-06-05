using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Membership
{
    public class MemberBillGenerationDetailsBO : PMMemberPaymentLedgerBO
    {
        public long MemberBillDetailsId { get; set; }
        public long MemberBillId { get; set; }
        //public long MemberPaymentId { get; set; }
        //public int BillId { get; set; }
        public decimal Amount { get; set; }
        public decimal PaymentAmount { get; set; }
        //public decimal DueAmount { get; set; }
    }
}
