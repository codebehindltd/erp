using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Membership
{
    public class MemberBillGenerationViewBO
    {
        public MemberBillGenerationBO BillGeneration { get; set; }
        public List<MemberBillGenerationDetailsBO> BillGenerationDetails { get; set; }
        public List<MemberPaymentLedgerVwBo> MemberBill = new List<MemberPaymentLedgerVwBo>();
    }
}
