using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SMSiteSurveyFeedbackDetailsBO
    {
        public long Id { get; set; }
        public long SiteSurveyFeedbackId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string UnitHead { get; set; }
        public decimal Quantity { get; set; }
    }
}
