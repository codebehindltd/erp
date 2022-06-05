using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesAndMarketing
{
    public class SalesCallViewBO : SMCompanySalesCallBO
    {
        public int EmpId { get; set; }
        public string CompanyAddress { get; set; }
        public string ShowFollowupDate { get; set; }
        public string SiteName { get; set; }
    }
}
