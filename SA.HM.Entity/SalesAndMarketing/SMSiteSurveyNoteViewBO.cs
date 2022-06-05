using HotelManagement.Entity.HotelManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMSiteSurveyNoteViewBO
    {
        public SMSiteSurveyNoteBO SiteSurveyNote { get; set; }
        public ContactInformationBO ContactInformation { get; set; }
        public GuestCompanyBO GuestCompany { get; set; }
        public SMDeal Deal { get; set; }
    }
}
