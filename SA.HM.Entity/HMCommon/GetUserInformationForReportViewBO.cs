using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class GetUserInformationForReportViewBO
    {
        public string GroupName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserDesignation { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
    }
}
