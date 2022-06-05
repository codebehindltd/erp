using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class ContactInformationBO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public string ContactNo { get; set; }
        public Nullable<int> ContactOwnerId { get; set; }
        public string ContactOwner { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string SourceName { get; set; }
        public string JobTitle { get; set; }

        public string Email { get; set; }
        public Nullable<System.DateTime> LastContactDateTime { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedDisplayDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        
        //new add
        public Nullable<int> SourceId { get; set; }
        public Nullable<int> LifeCycleId { get; set; }
        public string LifeCycleStage { get; set; }
        public string ContactType { get; set; }
        public string Department { get; set; }
        public string TicketNo { get; set; }

        public string MobilePersonal { get; set; }
        public string MobileWork { get; set; }
        public string PhonePersonal { get; set; }
        public string PhoneWork { get; set; }

        public string Facebook { get; set; }
        public string Skype { get; set; }
        public string Whatsapp { get; set; }
        public string Twitter { get; set; }
        public string EmailWork { get; set; }

        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<System.DateTime> DateAnniversary { get; set; }
        List<ContactInformationBO> CompanyContacts = new List<ContactInformationBO>();
        public string WorkAddress { get; set; }
        public string WorkCountry { get; set; }
        public string WorkState { get; set; }
        public string WorkCity { get; set; }
        public int? WorkCountryId { get; set; }
        public int? WorkStateId { get; set; }
        public int? WorkCityId { get; set; }
        public int? WorkLocationId { get; set; }
        public string WorkStreet { get; set; }
        public string WorkPostCode { get; set; }
        public string PersonalAddress { get; set; }
        public string PersonalCountry { get; set; }
        public string PersonalCity { get; set; }
        public string PersonalState { get; set; }
        public int? PersonalCountryId { get; set; }
        public int? PersonalStateId { get; set; }
        public int? PersonalCityId { get; set; }
        public int? PersonalLocationId { get; set; }
        public string PersonalStreet { get; set; }
        public string PersonalPostCode { get; set; }
        public string AccountManager { get; set; }
        public int IsDetailPanelEnableForContact { get; set; }
        public int IsDetailPanelEnableForParentCompany { get; set; }
    }
}
