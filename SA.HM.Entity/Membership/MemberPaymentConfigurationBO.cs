using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Membership
{
    public class MemberPaymentConfigurationBO
    {
        public long MemPaymentConfigId { get; set; }
        public string TransactionType { get; set; }
        public long MemberTypeOrMemberId { get; set; }
        public string BillingPeriod { get; set; }
        public decimal BillingAmount { get; set; }
        public DateTime? BillingStartDate { get; set; }
        public DateTime? DoorAccessDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        
        public string MemberName { get; set; }        
    }
}
