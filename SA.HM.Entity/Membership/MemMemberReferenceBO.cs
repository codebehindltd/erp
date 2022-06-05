using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Membership
{
    public class MemMemberReferenceBO
    {
        public int ReferenceId { get; set; }
        public int MemberId { get; set; }
        public string Arbitrator { get; set; }
        public string ArbitratorMode { get; set; }
        public string Relationship { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
