using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Membership
{
    public class OnlineMemberBO : MemMemberBasicsBO
    {
        public string NameBangla { get; set; }
        
        public string NickName { get; set; }
        public int? CountryId { get; set; }
        public string Country { get; set; } 

        public int? ProfessionId { get; set; }
        public string Profession { get; set; }
        public string Hobbies { get; set; }
        public string Awards { get; set; } 
        public bool IsOnlineMember { get; set; }
        public int? Introducer_1_id { get; set; }
        public string Introducer_1_Name { get; set; }
        public int? Introducer_2_id { get; set; }
        public string Introducer_2_Name { get; set; }
        public string Remarks { get; set; }
        
        public bool IsAccepted { get; set; }
        public bool IsRejected { get; set; }
        public bool IsDeferred { get; set; }
        //new add
        public string BirthPlace { get; set; }

        public bool IsAccepted1 { get; set; }
        public bool IsRejected1 { get; set; }
        public bool IsDeferred1 { get; set; }
        public bool IsAccepted2 { get; set; }
        public bool IsRejected2 { get; set; }
        public bool IsDeferred2 { get; set; }

        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }

        public string IntroducerMemNo1 { get; set; }
        public string IntroducerMemType1 { get; set; }
        public string IntroducerMobileNo1 { get; set; }
        public string IntroducerEmail1 { get; set; }

        public string IntroducerMemNo2 { get; set; }
        public string IntroducerMemType2 { get; set; }
        public string IntroducerMobileNo2 { get; set; }
        public string IntroducerEmail2 { get; set; } 

        public DateTime? MeetingDate { get; set; }
        public string MeetingDecision { get; set; }
        public DateTime? MeetingDateEC { get; set; }
        public string MeetingDecisionEC { get; set; }

        public string PathPersonalImg { get; set; }
        public string PathNIdImage { get; set; }
  
    }
}
