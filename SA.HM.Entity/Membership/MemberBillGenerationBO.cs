using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Membership
{
    public class MemberBillGenerationBO : MemberBillGenerationDetailsBO
    {
        //public long MemberBillId { get; set; }
        //public int MemberId { get; set; }
        public DateTime BillDate { get; set; }
        public string MemberBillNumber { get; set; }
        public string ApprovedStatus { get; set; }
        public string BillStatus { get; set; }
        public int BillCurrencyId { get; set; }
        //public string Remarks { get; set; }
        //public int CreatedBy { get; set; }
        //public int LastModifiedBy { get; set; }
        //public string MemberName { get; set; }
        //public string FullName { get; set; }
    }
}
