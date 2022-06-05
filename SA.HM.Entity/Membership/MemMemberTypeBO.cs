using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Membership
{
    public class MemMemberTypeBO
    {
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal? SubscriptionFee { get; set; }
        public decimal? DiscountPercent { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
