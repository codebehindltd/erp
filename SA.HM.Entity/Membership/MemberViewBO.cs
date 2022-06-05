using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Membership
{
    public class MemberViewBO
    {
        public int MemberId { get; set; }
        public int MappingMemberId { get; set; }
        public string MemberName { get; set; }
        public String MappingMemberName { get; set; }
        public List<MemMemberBasicsBO> Members { get; set; }
        public List<MemMemberBasicsBO> MappingMembers { get; set; }
    }
}
