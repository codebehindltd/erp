using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Membership
{
    public class MemberPaymentViewBO
    {
        public MemberPaymentBO MemberPayment { get; set; }
        public List<MemberPaymentDetailsBO> MemberPaymentDetails { get; set; }
        public MemMemberBasicsBO Member = new MemMemberBasicsBO();

        public List<MemberPaymentLedgerVwBo> MemberGeneratedBill = new List<MemberPaymentLedgerVwBo>();
        public List<MemberPaymentLedgerVwBo> MemberBill = new List<MemberPaymentLedgerVwBo>();
    }
}
