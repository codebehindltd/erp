using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SalesCallEntryBO
    {
        public long Id { get; set; }
        public string LogType { get; set; }
        public Nullable<System.DateTime> MeetingDate { get; set; }
        public string MeetingLocation { get; set; }
        public string ParticipantFromParty { get; set; }
        public string MeetingAgenda { get; set; }
        public string Decission { get; set; }
        public string MeetingAfterAction { get; set; }
        public string EmailType { get; set; }
        public string MeetingType { get; set; }
        public Nullable<int> SocialMediaId { get; set; }
        public string MessageType { get; set; }
        public string CallStatus { get; set; }
        public string LogBody { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<long> DealId { get; set; }
        public Nullable<long> ContactId { get; set; }
        public System.DateTime LogDate { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }

        public string LogDescription { get; set; }
        public string IsAdminUser { get; set; }
        public string MessagengerId { get; set; }
        public int? AccountManagerId { get; set; }
        public string AccountManager { get; set; }
        public string CompanyParticipant { get; set; }
        public string CompanyName { get; set; }
        public string IndustryName { get; set; }

        public string LogDateString { get; set; }
        public long? CallToActionId { get; set; }
        public List<SalesCallParticipantBO> ParticipantFromClient { get; set; }

    }
}
