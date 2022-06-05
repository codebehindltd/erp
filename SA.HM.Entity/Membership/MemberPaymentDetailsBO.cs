using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Membership
{
    public class MemberPaymentDetailsBO : MemberPaymentBO
    {
        public Int64? PaymentDetailsId { get; set; }
        //public Int64 PaymentId { get; set; }
        public Int64? MemberBillDetailsId { get; set; }
        //public Int64 MemberPaymentId { get; set; }
        //public Int64 BillId { get; set; }
        public decimal? PaymentAmount { get; set; }
    }
}
