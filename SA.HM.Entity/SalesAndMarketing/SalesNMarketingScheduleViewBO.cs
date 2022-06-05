using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SalesNMarketingScheduleViewBO
    {
        public string InitialDate { get; set; }
        public string FollowUpDate { get; set; }
        public string SalesPerson { get; set; }
        public string Participants { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string FollowupType { get; set; }
        public string Purpose { get; set; }
        public string Remarks { get; set; }
    }
}
