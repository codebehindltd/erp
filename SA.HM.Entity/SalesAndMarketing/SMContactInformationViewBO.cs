using HotelManagement.Entity.HMCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMContactInformationViewBO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ContactOwner { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Industry { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyWebsite { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyLifeCycleStage { get; set; }
        public string ContactLifeCycleStage { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string EmailWork { get; set; }
        public string Department { get; set; }

        public string MobilePersonal { get; set; }
        public string MobileWork { get; set; }
        public string PhonePersonal { get; set; }
        public string PhoneWork { get; set; }

        public string Facebook { get; set; }
        public string Skype { get; set; }
        public string Whatsapp { get; set; }
        public string Twitter { get; set; }
        public string PersonalAddress { get; set; }
        public string WorkAddress { get; set; }

        public string SocialMedia { get; set; }
        public string Website { get; set; }

        public DateTime? DOB { get; set; }
        public DateTime? DateAnniversary { get; set; }
        public DateTime LastActivityDateTime { get; set; }

        public virtual List<SMDealView> Deals { get; set; }
        public virtual List<DocumentsBO> Documents { get; set; }
        public virtual List<SMContactInformationViewBO> PastCompanys { get; set; }


    }
}
