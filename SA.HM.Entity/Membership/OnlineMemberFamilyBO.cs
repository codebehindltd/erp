using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Membership
{
    public class OnlineMemberFamilyBO : MemMemberFamilyMemberBO
    {
        public DateTime? FamMemMarriageDate { get; set; }
        public int FamMemBloodGroupId { get; set; }
        public string FamMemBloodGroup { get; set; }
    } 
}
