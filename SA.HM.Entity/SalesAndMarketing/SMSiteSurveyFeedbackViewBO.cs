using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMSiteSurveyFeedbackViewBO
    {
        public SMSiteSurveyFeedbackBO SMSiteSurveyFeedbackBO { get; set; }
        public List<SMSiteSurveyFeedbackDetailsBO> SMSiteSurveyFeedbackDetailsBOList { get; set; }
        public List<int> SMSiteSurveyEngineerBOList { get; set; }
    }
}
