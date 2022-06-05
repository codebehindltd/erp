using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class GetApprovalConfigurationForReportViewBO
    {
        public string Features { get; set; }
        public string GroupName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserDesignation { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public Nullable<bool> IsCheckedBy { get; set; }
        public Nullable<bool> IsApprovedBy { get; set; }

    }
}
