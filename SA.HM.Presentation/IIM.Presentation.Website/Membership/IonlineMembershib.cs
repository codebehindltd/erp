using HotelManagement.Entity.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagement.Presentation.Website.Membership
{
    public class IonlineMembershib
    {
        public List<OnlineMemberBO> onlineMember = new List<OnlineMemberBO>();
        public List<OnlineMemberEducationBO> educationBOs = new List<OnlineMemberEducationBO>();
        public List<OnlineMemberFamilyBO> familyBOs = new List<OnlineMemberFamilyBO>();
    }
}