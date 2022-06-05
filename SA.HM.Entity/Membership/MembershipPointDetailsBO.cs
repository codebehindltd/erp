using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Membership
{
    public class MembershipPointDetailsBO
    {
        public int MemberID { get; set; }
        public Decimal PaymentAmount { get; set; }
        public Decimal RedeemedAmount { get; set; }
        public Decimal PointWiseAmount { get; set; }
        public String PointType { get; set; }
        public int BillId { get; set; }
        public Decimal TotalPoint { get; set; }
        public string MemberCode { get; set; }
    }
}
